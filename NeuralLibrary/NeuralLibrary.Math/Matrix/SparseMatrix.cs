using System.Collections.Generic;
using System.IO;

namespace NeuralLibrary.Math.Matrix
{
    public class SparseMatrix : IMatrix
    {
        public int Row { get; set; }
        public int Column { get; set; }
        
        public List<Dictionary<int, double>> Data { get; set; }

        public SparseMatrix(int row, int col)
        {
            Row = row;
            Column = col;

            Data = new List<Dictionary<int, double>>();

            for (var i = 0; i < row; i++)
            {
                Data.Add(new Dictionary<int, double>());
            }
        }

        public SparseMatrix(string fileName)
        {
            // get cols
            var srCount = new StreamReader(fileName);
            var firstLine = srCount.ReadLine();
            if (firstLine != null)
            {
                Column = int.Parse(firstLine);
            }

            // count rows
            Row = 0;
            while (srCount.ReadLine() != null)
            {
                Row++;
            }
            srCount.Close();

            // init matrix
            Data = new List<Dictionary<int, double>>();

            // TODO:fill matrix
            var srData = new StreamReader(fileName);
            srData.ReadLine();

            string line;
            while ((line = srData.ReadLine()) != null)
            {
                var lineArray = line.Split('\t');
                for (var i = 0; i < lineArray.Length; i++)
                {
                    //Data.Add(new Tuple<int, int, double>
                        //(int.Parse(lineArray[0]), int.Parse(lineArray[1]), double.Parse(lineArray[2])));
                }
            }
            srData.Close();
        }

        public double this[int row, int column]
        {
            get
            {
                foreach (var r in Data[row])
                {
                    if (r.Key == column)
                        return r.Value;
                }
                return 0.0;
            }
            set
            {
                foreach (var r in Data[row])
                {
                    if (r.Key == column)
                    {
                        Data[row].Remove(r.Key);
                        break;
                    }
                }

                Data[row].Add(column, value);
            }
        }
    }
}
