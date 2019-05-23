//Версия 1.0.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eshiba
{
    using int8_t = System.Byte;
    using boolean = System.Boolean;
    using uint8_t = System.Byte;
    class Core
    {
        /// <summary>
        /// Сравнение массивов
        /// </summary>
        /// <param name="arr0">массив 1</param>
        /// <param name="arr1">массив 2</param>
        /// <param name="length">длина массива</param>
        /// <returns></returns>
        public static bool CpByteArr(byte [] arr0, byte [] arr1, int length)
        {
            int a = 0;
            for(int i =0; i < length; i++)
            {
                if (arr0[i] == arr1[i]) a++;
            }
            if (a == length) return true;
            return false;
        }
        /// <summary>
        /// Сравнение массивов
        /// </summary>
        /// <param name="arr0">массив 1</param>
        /// <param name="arr1">массив 2</param>
        /// <returns></returns>
        public static bool CpByteArr(byte[] arr0, byte[] arr1)
        {
            int length = arr0.Length;
            int a = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr0[i] == arr1[i]) a++;
            }
            if (a == length) return true;
            return false;
        }
    }
}
