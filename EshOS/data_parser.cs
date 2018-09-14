using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Esh
{
    static public class data_parser
    {
        public static int Byte4toInt32(byte[] data)
        {
            return BitConverter.ToInt32(data, 0);
        }

        public static int Byte4toInt32(byte[] data, int index)
        {
            return BitConverter.ToInt32(data, index);
        }

        public static byte[] IntTo4Byte(int a)
        {
            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes(a);
            return buffer;
        }

        public static byte[] IntTo4Byte(int a, byte[] data, int index)
        {
            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes(a);
            for(int i = 0; i < 4; i++) { data[i + index] = buffer[i]; }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b">Массив</param>
        /// <param name="c">индекс начала записи</param>
        /// <param name="a">конвертируемое число</param>
        /// <returns></returns>
        public static byte[] IntTo4Byte(byte[] b, int c, int a)
        {
            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes(a);

            b[c] = buffer[0];
            b[c + 1] = buffer[1];
            b[c + 2] = buffer[2];
            b[c + 3] = buffer[3];

            return buffer;
        }

        static public byte[] StrToByte(string str)
        {
            byte[] arr = Encoding.GetEncoding(1251).GetBytes(str);
            return arr;
        }


        static public string ByteToStr(byte[] arr)
        {
            string str = Encoding.GetEncoding(1251).GetString(arr);
            return str;
        }

        static public string Sha1BytetoStr(byte[] arr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in arr)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        public static byte[] GetSha(string inputString)
        {
            HashAlgorithm algorithm = SHA1.Create();  // SHA1.Create()
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

    }
}
