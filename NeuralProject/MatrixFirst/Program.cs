using NeuralLibrary.Core;
using NeuralLibrary.Math.Matrix;

namespace MatrixFirst
{
    public class Program
    {
        static void Main(string[] args)
        {
            var target = "first";

            var input = new SimpleMatrix(@"D:\zoneA\" + target + "_input.csv");
            var output = new SimpleMatrix(@"D:\zoneA\" + target + "_output.csv");

            var net = new ForeNetwork(input, output, 1500);
            net.Train(0.001, 15, 0.035, @"D:\zoneA\" + target + "_errorRate.txt");
            //Console.WriteLine("tt:" + net.ActualIterNum);

            net.WriteWeightMat(@"D:\zoneA\" + target);


        }
    }
}
