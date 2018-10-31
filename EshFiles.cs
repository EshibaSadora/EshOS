using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace Esh
{
    public class Files
    {

        string path;
        string[] pathes;

        Files(string _path)
        {
            path = _path;
        }

        Files(string [] _path)
        {
            pathes = _path;
        }

        Files()
        {

        }

        public string readtxt()
        {
            string str = "";
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    str += line + "\n";
                }
            }
            return str;
        }

        public void writetxt(string str)
        {
            using (StreamWriter sr = new StreamWriter(path))
            {
                sr.Write(str);
            }
        }

        public static void writetxt(string path, string str)
        {
            using (StreamWriter sr = new StreamWriter(path))
            {
                sr.Write(str);
            }
        }

        public static void WritelineTotxt(string path, string str)
        {
            using (StreamWriter sr = new StreamWriter(path))
            {
                sr.WriteLine(str);
            }
        }
    }
}
