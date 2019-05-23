//Версия 1.0.1

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

        /// <summary>
        /// LH
        /// </summary>
        /// <param name="var"></param>
        /// <returns></returns>
        public static byte[] Int16To2Byte(UInt16 var)
        {
            byte[] ba = BitConverter.GetBytes(var);
            return ba;
        }

        public static void Int16To2Byte(UInt16 var,ref Byte L,ref Byte H)
        {
            byte[] ba = BitConverter.GetBytes(var);
            L = ba[0];
            H = ba[1];
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
        /// Конвертирует массив 2 байтов в Int16
        /// </summary>
        /// <param name="data">массив</param>
        /// <returns></returns>
        public static int Byte2ToInt16(byte a, byte b)
        {
            return BitConverter.ToInt16(new byte[] { a,b}, 0);
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


        /// <summary>
        /// Получает массив Char из Строки
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="length">Длина строки</param>
        /// <returns></returns>
        static public char[] StrToChar(string input, int length)
        {
            Char[] out_ = new char[length];

            out_[0] = Convert.ToChar(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                out_[i+1] = input[i];
            }

            return out_;
        }


        /// <summary>
        /// Получает строку из массива Char
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        static public string CharToStr(char [] input)
        {
            int lenght = Convert.ToInt32(input[0]);
            char[] str = new char[lenght];

            for (int i = 0; i < lenght; i++)
            {
                str[i] = input[i + 1];
            }

            return new string(str);
        }

        /// <summary>
        /// Получает массив байт из строки
        /// </summary>
        /// <param name="input">Входная строка</param>
        /// <param name="length">Длина строки</param>
        /// <returns></returns>
        static public byte[] StrToByte(string input, int length)
        {
            char[] str = StrToChar(input, length);
            byte[] bytestr = new byte[length * 2];
            for(int i = 0; i < str.Length; i++)
            {
                byte[] arr = BitConverter.GetBytes(str[i]);
                bytestr[i * 2] = arr[0];
                bytestr[i * 2 + 1] = arr[1];
            }
            return bytestr;
        }

        /// <summary>
        /// Получает строку из массива байтов
        /// </summary>
        /// <param name="input">Входной массив</param>
        /// <param name="length">Длина строки</param>
        /// <returns></returns>
        static public string ByteToStr(byte [] input, int length)
        {
            char[] str = new char[length];
 

            for (int i = 0; i < str.Length/2; i++)
            {
                str[i] = Convert.ToChar(BitConverter.ToChar(new byte[] { input[i * 2], input[i * 2 + 1] }, 0));
            }

            return CharToStr(str);

        }



        public static System.Collections.BitArray ToBits(byte var)
        {
            System.Collections.BitArray F = new System.Collections.BitArray(8);
            F[0] = (bool)(((byte)var & (1 << 7)) != 0);
            F[1] = (bool)(((byte)var & (1 << 6)) != 0);
            F[2] = (bool)(((byte)var & (1 << 5)) != 0);
            F[3] = (bool)(((byte)var & (1 << 4)) != 0);
            F[4] = (bool)(((byte)var & (1 << 3)) != 0);
            F[5] = (bool)(((byte)var & (1 << 2)) != 0);
            F[6] = (bool)(((byte)var & (1 << 1)) != 0);
            F[7] = (bool)(((byte)var & (1 << 0)) != 0);
            return F;
        }

        public static Byte ToByte(System.Collections.BitArray arr)
        {
            return Convert.ToByte(arr);
        }

        public static uint ColorToUint32(System.Drawing.Color col)
        {
            return BitConverter.ToUInt32(new byte[] { col.B, col.G, col.R, 0xFF }, 0);
        }

        public static UInt4[] ByteTo2Uint4(byte value)
        {
            System.Collections.BitArray[] arr = new System.Collections.BitArray[2];

            System.Collections.BitArray boolarr = ToBits(value);

            arr[0] = new System.Collections.BitArray(4);
            arr[1] = new System.Collections.BitArray(4);

            for (int i = 0; i < 4; i++)
            {
                arr[0][i] = boolarr[i];
                arr[1][i] = boolarr[i + 4];
            }

            UInt4[] ret = new UInt4[2];

            ret[0] = arr[1];
            ret[1] = arr[0];

            return ret;

        }
            public static void ByteTo2Uint4(byte value, ref UInt4 L, ref UInt4 H)
            {
                System.Collections.BitArray[] arr = new System.Collections.BitArray[2];

                System.Collections.BitArray boolarr = ToBits(value);

                arr[0] = new System.Collections.BitArray(4);
                arr[1] = new System.Collections.BitArray(4);

                for (int i = 0; i < 4; i++)
                {
                    arr[0][i] = boolarr[i];
                    arr[1][i] = boolarr[i + 4];
                }

                UInt4[] ret = new UInt4[2];

                H = arr[1];
                L = arr[0];
            }


        }
}
