using System.Collections.Generic;
using System.IO;

namespace NeuralLibrary.Tool
{
    public static class FileTools
    {
        public static void WriteListToFile(IList<double> list,string path)
        {
            var fs = new FileStream(path, FileMode.Append);
            var sw = new StreamWriter(fs);

            foreach (var a in list)
            {
                sw.WriteLine(a);
            }

            sw.Close();
            fs.Close();
        }
    }
}
