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


        public Files(string _path)
        {
            path = _path;
        }

        public Files(string [] _path)
        {
            pathes = _path;
        }

        public Files()
        {

        }

        public static string Readtxt(string path)
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

        public static string Readtxt(string path,Encoding enc)
        {
            string str = "";
            using (StreamReader sr = new StreamReader(path,enc))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    str += line + "\n";
                }
            }
            return str;
        }

        public void Writetxt(string str)
        {
            using (StreamWriter sr = new StreamWriter(path,false))
            {
                sr.Write(str);
            }
        }

        public static void Writetxt(string path, string str)
        {
            using (StreamWriter sr = new StreamWriter(path, false))
            {
                sr.Write(str);
            }
        }

        public static void Writetxt(string path, string str, Encoding enc)
        {
            using (StreamWriter sr = new StreamWriter(path, false, enc))
            {
                sr.Write(str);
            }
        }



        public static void WritelineTotxt(string path, string str)
        {
            using (StreamWriter sr = new StreamWriter(path, false, Encoding.GetEncoding(1251)))
            {
                sr.WriteLine(str);
            }
        }
    }
}
