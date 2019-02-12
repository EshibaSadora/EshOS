using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Eshiba
{
    static public class DataConverter
    {
        /// <summary>
        /// Конвертирует массив 4 байтов в Int32
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int Byte4toInt32(byte[] data)
        {
            return BitConverter.ToInt32(data, 0);
        }


        /// <summary>
        /// Конвертирует массив 4 байтов в Int32
        /// </summary>
        /// <param name="data">массив</param>
        /// <param name="index">Индекс первого числа в массиве</param>
        /// <returns></returns>
        public static int Byte4toInt32(byte[] data, int index)
        {
            return BitConverter.ToInt32(data, index);
        }

        public static byte[] Int16To2Byte(UInt16 var)
        {
            byte[] ba = BitConverter.GetBytes(var);
            return ba;
        }

        /// <summary>
        /// Конвертирует массив 2 байтов в Int16
        /// </summary>
        /// <param name="data">массив</param>
        /// <returns></returns>
        public static int Byte2ToInt16(byte[] data)
        {
            return BitConverter.ToInt16(data, 0);
        }

        /// <summary>
        /// Конвертирует число Int32 в массив из 4 байт. 
        /// </summary>
        /// <param name="a">число</param>
        /// <returns></returns>
        public static byte[] IntTo4Byte(int a)
        {
            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes(a);
            return buffer;
        }


        /// <summary>
        /// Конвертирует число Int16 в массив из 2 байт. 
        /// </summary>
        /// <param name="a">число</param>
        /// <returns></returns>
        public static byte[] IntTo4Byte(Int16 a)
        {
            byte[] buffer = new byte[2];
            buffer = BitConverter.GetBytes(a);
            return buffer;
        }

        /// <summary>
        /// Записывает число Int32 в массив Byte;
        /// </summary>
        /// <param name="a">число</param>
        /// <param name="data">записываемый массив</param>
        /// <param name="index">Индекс первого байта</param>
        /// <returns></returns>
        public static byte[] IntTo4Byte(int a, byte[] data, int index)
        {
            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes(a);
            for(int i = 0; i < 4; i++) { data[i + index] = buffer[i]; }
            return data;
        }

        static public sbyte ByteTOsbyte(byte var)
        {
            return unchecked((sbyte)var);
        }

        /// <summary>
        /// Конввертирует два байта в формате 0xFF в Int16
        /// </summary>
        /// <param name="MSB">Старший байт</param>
        /// <param name="LSB">младший байт</param>
        /// <returns></returns>
        static public UInt16 bytes_to_u16(byte MSB, byte LSB)
        {

            return Convert.ToUInt16((MSB & 255) << 8 | LSB & 255);
           
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

        /// <summary>
        /// Конвертипует строку в масисв байтов
        /// </summary>
        /// <param name="str">строка</param>
        /// <returns></returns>
        static public byte[] StrToByte(string str)
        {
            byte[] arr = Encoding.GetEncoding(1251).GetBytes(str);
            return arr;
        }

        /// <summary>
        /// Конвертипует строку и записывает его в массив байтов
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="data">массив</param>
        /// <param name="begin_index">индекс первого числа</param>
        /// <returns></returns>
        static public byte[] StrToByte(string str,byte [] data, int begin_index)
        {
            byte[] arr = Encoding.GetEncoding(1251).GetBytes(str);
            
            for (int i = 0; i < arr.Length; i++)
            {
                data[begin_index + i] = arr[i];
            }

            return data;
            
        }

        /// <summary>
        /// Конвертирует массив байтов в строку
        /// </summary>
        /// <param name="arr">массив байтов</param>
        /// <returns></returns>
        static public string ByteToStr(byte[] arr)
        {
            string str = Encoding.GetEncoding(1251).GetString(arr);
            return str;
        }

        /// <summary>
        /// Ищет строку в массиве байтов и возвращает её
        /// </summary>
        /// <param name="arr">массив</param>
        /// <param name="begin">индекс первого байта строки</param>
        /// <param name="length">длина строки</param>
        /// <returns></returns>
        static public string ByteToStr(byte[] arr, int begin, int length)
        {
            byte[] arr_new = new byte[length];
            for (int i = 0; i < length; i++)
            {
                arr_new[i] = arr[i + begin];
            }

            string str = Encoding.GetEncoding(1251).GetString(arr_new);
            return str;
        }

        /// <summary>
        /// Конвертирует Byte SHA1 в строку
        /// </summary>
        /// <param name="arr">SHA1Byte</param>
        /// <returns></returns>
        static public string Sha1BytetoStr(byte[] arr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in arr)
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// Конвертирует строку в SHA!
        /// </summary>
        /// <param name="inputString">строка</param>
        /// <returns></returns>
        public static byte[] GetSha(string inputString)
        {
            HashAlgorithm algorithm = SHA1.Create();  // SHA1.Create()
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }


        /// <summary>
        /// Переносит один массив в другой 
        /// </summary>
        /// <param name="Arr_In">Массив в который переносит</param>
        /// <param name="Arr_from">Массив из которого переносит</param>
        /// <param name="begin_index">Индекс первого байта массива в который переносят</param>
        public static void ByteToByte(byte [] Arr_In, byte [] Arr_from, int begin_index)
        {
            for (int i = 0; i < Arr_from.Length; i++)
            {
                Arr_In[begin_index + i] = Arr_from[i];
            }
        }
		
		        /// <summary>
        /// Преобразует в Float
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static float ToFloat(string str)
        {
            str = str.Replace('.', ',');
            return Convert.ToSingle(str);
        }

    }
}
