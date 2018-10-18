using System;
using System.IO;

namespace NeuralLibrary.Math.Matrix
{
    public class SimpleMatrix : IMatrix
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public double[,] Data { get; set; }

        public SimpleMatrix(int row, int col)
        {
            // init matrix
            Row = row;
            Column = col;
            Data = new double[row, col];

            for (var i = 0; i < row; i++)
            {
                for (var j = 0; j < col; j++)
                {
                    Data[i, j] = 0;
                }
            }
        }

        public SimpleMatrix(string fileName)
        {
            //var splitUnit = '\t';
            var splitUnit = ',';

            // count columns
            var srCol = new StreamReader(fileName);
            var firstLine = srCol.ReadLine();
            if (firstLine != null)
            {
                var firstArray = firstLine.Split(splitUnit);
                Column = firstArray.Length;
            }
            srCol.Close();

            // count rows
            Row = 0;
            var srRow = new StreamReader(fileName);
            while (srRow.ReadLine() != null)
            {
                Row++;
            }
            srRow.Close();

            // init matrix
            Data = new double[Row, Column];

            // fill matrix
            var srData = new StreamReader(fileName);
            var curRow = 0;
            string line;
            while ((line = srData.ReadLine()) != null)
            {
                var lineArray = line.Split(splitUnit);
                for (var i = 0; i < lineArray.Length; i++)
                {
                    Data[curRow, i] = double.Parse(lineArray[i]);
                }
                curRow++;
            }
            srData.Close();
        }

        public void WriteTo(string path)
        {
            var fs = new FileStream(path, FileMode.Create);
            var sw = new StreamWriter(fs);

            for (var i = 0; i < Row; i++)
            {
                sw.WriteLine(this.RowVector(i).ToSingleLineRow());
            }

            sw.Close();
            fs.Close();
        }

        public double this[int row, int column]
        {
            get { return Data[row, column]; }
            set { Data[row, column] = value; }
        }

        public void Randomize()
        {
            var rm = new Random();

            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    Data[i, j] = rm.Next(-100, 100) / 7000.0;
                }
            }
        }

        public SimpleMatrix RowVector(int row)
        {
            var result = new SimpleMatrix(1, Column);

            for (var i = 0; i < Column; i++)
            {
                result[0, i] = Data[row, i];
            }

            return result;
        }

        public static SimpleMatrix operator -(SimpleMatrix a, SimpleMatrix b)
        {
            var minus = new SimpleMatrix(a.Row, a.Column);

            for (var i = 0; i < minus.Row; i++)
            {
                for (var j = 0; j < minus.Column; j++)
                {
                    minus[i, j] = a[i, j] - b[i, j];
                }
            }
            return minus;
        }

        public static SimpleMatrix operator +(SimpleMatrix a, SimpleMatrix b)
        {
            var add = new SimpleMatrix(a.Row, a.Column);

            for (var i = 0; i < add.Row; i++)
            {
                for (var j = 0; j < add.Column; j++)
                {
                    add[i, j] = a[i, j] + b[i, j];
                }
            }
            return add;
        }

        public void LimitMax(double max)
        {
            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    if (Data[i, j] > max)
                        Data[i, j] = max;
                }
            }
        } 

        public static double Norm2(SimpleMatrix a)
        {
            var norm = 0.0;
            for (var i = 0; i < a.Row; i++)
            {
                for (var j = 0; j < a.Column; j++)
                {
                    norm += a[i, j] * a[i, j];
                }
            }
            return System.Math.Sqrt(norm);
        }

        public void WriteMat()
        {
            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    Console.Write("   $ " + this[i, j]);
                }
                Console.Write("\n");
            }
        }

        public override string ToString()
        {
            var str = "";

            for (var i = 0; i < Row; i++)
            {
                for (var j = 0; j < Column; j++)
                {
                    str += "    " + this[i, j];
                }
                str += "  ||  ";
            }

            return str;
        }

        public string ToSingleLine()
        {
            var str = this[0, 0].ToString();
            //for (var i = 0; i < Row; i++)
            //{
            for (var i = 1; i < Row; i++)
            {
                str += "," + this[i, 0];
            }
            //}
            return str;
        }

        public string ToSingleLineRow()
        {
            var str = this[0, 0].ToString();
            //for (var i = 0; i < Row; i++)
            //{
            for (var i = 1; i < Column; i++)
            {
                str += "," + this[0, i];
            }
            //}
            return str;
        }

        public static SimpleMatrix Link(SimpleMatrix vector1, SimpleMatrix vector2)
        {
            // check is vector
            if (vector1.Column != 1 || vector2.Column != 1)
                return null;

            var newRow = vector1.Row + vector2.Row;
            var result = new SimpleMatrix(newRow, 1);

            for (var i = 0; i < vector1.Row; i++)
            {
                result[i, 0] = vector1[i, 0];
            }

            for (var i = 0; i < vector2.Row; i++)
            {
                result[vector1.Row + i, 0] = vector2[i, 0];
            }

            return result;
        }

        public static SimpleMatrix SingleValueMatrix(double val)
        {
            return new SimpleMatrix(1, 1) { Data = { [0, 0] = val } };
        }

        public static double MaxAbsMember(SimpleMatrix a)
        {
            var max = -1.0;
            foreach (var d in a.Data)
            {
                if (System.Math.Abs(d) > max)
                    max = System.Math.Abs(d);
            }
            return max;
        }

        public static SimpleMatrix Transpose(SimpleMatrix a)
        {
            var result = new SimpleMatrix(a.Column, a.Row);

            for (var i = 0; i < result.Row; i++)
            {
                for (var j = 0; j < result.Column; j++)
                {
                    result[i, j] = a[j, i];
                }
            }

            return result;
        }
    }
}
