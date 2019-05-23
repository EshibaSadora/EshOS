//Версия 1.0.1

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


    public class UInt4
    {
        int value = 0;

        public const int MaxValue = 15;
        public const int MinValue = 0;

        public UInt4() { checker(0); }
        public UInt4(int var) { checker((int)var); }
        public UInt4(byte var) { checker((int)var); }
        public UInt4(short var) { checker((int)var); }
        public UInt4(uint var) { checker((int)var); }
        public UInt4(ushort var) { checker((int)var); }

        public UInt4 Get(int var)
        {
            checker(var);
            return this;
        }

        void checker(int var)
        {
            if (var > MaxValue) throw new Exception("Число превышает максиальное значение");
            if (var < MinValue) throw new Exception("Число меньше минимального значения");
            value = var;
        }

        public static implicit operator int(UInt4 tst)
        {
            return tst.value;
        }

        public static implicit operator UInt4(int tst)
        {
            return new UInt4(tst);
        }


        public static implicit operator UInt4(System.Collections.BitArray arr)
        {
            if (arr.Length != 4) throw new Exception("Массив битов должен содержать 4 числа");

            UInt4 var = new UInt4();

            if (arr[3] == true) var.value = var.value + 1;
            if (arr[2] == true) var.value = var.value + 2;
            if (arr[1] == true) var.value = var.value + 4;
            if (arr[0] == true) var.value = var.value + 8;

            return var;
        }

        public System.Collections.BitArray toBitArr()
        {
            System.Collections.BitArray arr = new System.Collections.BitArray(4);
            if (value - 8 > 0) { arr[0] = true; value = value - 8; }
            if (value - 4 > 0) { arr[1] = true; value = value - 4; }
            if (value - 2 > 0) { arr[2] = true; value = value - 2; }
            if (value - 1 > 0) { arr[3] = true; value = value - 1; }
            return arr;
        }


    }
}
