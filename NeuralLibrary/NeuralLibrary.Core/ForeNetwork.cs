using System;
using System.Collections.Generic;

namespace NeuralLibrary.Core
{
    public class ForeNetwork
    {
        public SimpleMatrix InputData { get; set; }
        public SimpleMatrix OutputData { get; set; }
        public int SampleSize { get; set; }
        public int InputNumber { get; set; }
        public int OutputNumber { get; set; }
        public SimpleMatrix WeightInput { get; set; }
        public SimpleMatrix WeightOutput { get; set; }
        public int HiddenNumber { get; set; }
        public int MaxIterNum { get; set; }
        public int ActualIterNum { get; set; }
        public bool Trained { get; set; }

        public ForeNetwork()
        { }

        public ForeNetwork(SimpleMatrix input, SimpleMatrix output, int hiddenNum)
        {
            InputData = input;
            OutputData = output;
            HiddenNumber = hiddenNum;
            SampleSize = input.Row;
            InputNumber = input.Column;
            OutputNumber = output.Column;

            // init matrix
            WeightInput = new SimpleMatrix(InputNumber, HiddenNumber);
            WeightOutput = new SimpleMatrix(HiddenNumber, OutputNumber);
            WeightInput.Randomize();
            WeightOutput.Randomize();

            //MaxIterNum = 300;
            Trained = false;
        }

        public ForeNetwork(string baseWeightPath)
        {
            var wiPath = baseWeightPath + "_WI.csv";
            var woPath = baseWeightPath + "_WO.csv";

            WeightInput = new SimpleMatrix(wiPath);
            WeightOutput = new SimpleMatrix(woPath);

            HiddenNumber = WeightInput.Column;
            InputNumber = WeightInput.Row;
            OutputNumber = WeightOutput.Column;
        }

        public void Train(TrainingParam p, string savePath)
        {
            Train(p.Tolerence, p.MaxTime, p.Speed, savePath);
        }

        public void Train(double tol, int max, double speed, string savePath)
        {
            MaxIterNum = max;
            var errorRateList = new List<double>();
            var maxAverList = new List<double>();

            for (var iter = 0; iter < MaxIterNum; iter++)
            {
                Console.WriteLine("iter: " + iter);
                var curError = 0.0;
                var curMaxMember = 0.0;

                for (var sample = 0; sample < SampleSize; sample++)
                {
                    if (sample % 2 == 0)
                    {
                        Console.WriteLine("sample: " + sample + "/" + SampleSize);
                    }

                    // raw data
                    var curInput = InputData.RowVector(sample);
                    var curOutput = OutputData.RowVector(sample);

                    // init
                    var output = GetNetOutput(curInput);
                    var hVal = output.Item1;
                    var oVal = output.Item2;

                    var errorTuple = VectorErrorRate(oVal, curOutput);
                    var thisError = Math.Pow(errorTuple.Item1, 2);
                    curError += thisError;
                    curMaxMember += errorTuple.Item2;

                    var hDelta = new SimpleMatrix(1, HiddenNumber);
                    var oDelta = new SimpleMatrix(1, OutputNumber);

                    // delta of out
                    for (var kk = 0; kk < OutputNumber; kk++)
                    {
                        oDelta[0, kk] = curOutput[0, kk] - oVal[0, kk];
                    }

                    // delta of hidden
                    for (var kk = 0; kk < HiddenNumber; kk++)
                    {
                        var stepSum = 0.0;
                        for (var mm = 0; mm < OutputNumber; mm++)
                        {
                            stepSum += oDelta[0, mm] * WeightOutput[kk, mm];
                        }
                        hDelta[0, kk] = stepSum;
                    }

                    const double mu = 0.5;
                    var hPulse = new SimpleMatrix(InputNumber, HiddenNumber);
                    var oPulse = new SimpleMatrix(HiddenNumber, OutputNumber);

                    // refresh WI
                    for (var ii = 0; ii < InputNumber; ii++)
                    {
                        for (var jj = 0; jj < HiddenNumber; jj++)
                        {
                            //WeightInput[ii, jj] += speed * hDelta[0, jj] * curInput[0, ii];
                            hPulse[ii, jj] = mu * hPulse[ii, jj] + speed * hDelta[0, jj] * curInput[0, ii];
                            WeightInput[ii, jj] += hPulse[ii, jj];
                        }
                    }

                    // refresh WO
                    for (var ii = 0; ii < HiddenNumber; ii++)
                    {
                        for (var jj = 0; jj < OutputNumber; jj++)
                        {
                            //WeightOutput[ii, jj] += speed * oDelta[0, jj] * hVal[0, ii];
                            oPulse[ii, jj] = mu * oPulse[ii, jj] + speed * oDelta[0, jj] * hVal[0, ii];
                            WeightOutput[ii, jj] += oPulse[ii, jj];
                        }
                    }
                }

                var errorRate = Math.Sqrt(curError / SampleSize);
                var maxErrorMemberAver = curMaxMember / SampleSize;

                errorRateList.Add(errorRate);
                maxAverList.Add(maxErrorMemberAver);

                // TODO: decide out
                if (tol >= errorRate)  // use actual statement
                {
                    ActualIterNum = iter;
                    break;
                }
                Console.WriteLine("err:" + errorRate + "  Max Aver: " + maxErrorMemberAver + "\n");
            }

            FileTools.WriteListToFile(errorRateList, savePath);
            FileTools.WriteListToFile(maxAverList, savePath);
            Trained = true;
        }

        public Tuple<SimpleMatrix, SimpleMatrix> GetNetOutput(SimpleMatrix input)
        {
            var hVal = new SimpleMatrix(1, HiddenNumber);
            var oVal = new SimpleMatrix(1, OutputNumber);

            // get hidden values
            for (var ii = 0; ii < InputNumber; ii++)
            {
                for (var jj = 0; jj < HiddenNumber; jj++)
                {
                    hVal[0, jj] += input[0, ii] * WeightInput[ii, jj];
                }
            }

            // get out values
            for (var ii = 0; ii < HiddenNumber; ii++)
            {
                for (var jj = 0; jj < OutputNumber; jj++)
                {
                    oVal[0, jj] += hVal[0, ii] * WeightOutput[ii, jj];
                }
            }

            return new Tuple<SimpleMatrix, SimpleMatrix>(hVal, oVal);
        }

        private static Tuple<double, double> VectorErrorRate(SimpleMatrix output, SimpleMatrix answer)
        {
            var diff = output - answer;
            var b = SimpleMatrix.Norm2(diff);
            var c = SimpleMatrix.Norm2(answer);
            var d = b / c;
            return new Tuple<double, double>(d, SimpleMatrix.MaxAbsMember(diff));
        }

        public void WriteWeightMat(string basePath)
        {
            var wiPath = basePath + "_WI.csv";
            var woPath = basePath + "_WO.csv";

            WeightInput.WriteTo(wiPath);
            WeightOutput.WriteTo(woPath);
        }
    }
}
