//Версия 1.0.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eshiba
{
    class Bits
    {
        class Bit :  IComparable<bool>, IConvertible, IEquatable<bool>
        {
            static bool value = false;

            byte getvalue()
            {
                if (value == true) return 1;
                return 0;
            }

            public int CompareTo(bool var)
            {
                return getvalue();
            }

            public TypeCode GetTypeCode()
            {
                return TypeCode.Object;
            }

            bool IConvertible.ToBoolean(IFormatProvider provider)
            {
                return value;
            }


            byte IConvertible.ToByte(IFormatProvider provider)
            {
                return getvalue();
            }


            char IConvertible.ToChar(IFormatProvider provider)
            {
                if (value == true) return '1';
                return '0';
            }

            DateTime IConvertible.ToDateTime(IFormatProvider provider)
            {
                return new DateTime(getvalue());
            }

            decimal IConvertible.ToDecimal(IFormatProvider provider)
            {
                return getvalue();
            }

            double IConvertible.ToDouble(IFormatProvider provider)
            {
                return getvalue();
            }

            short IConvertible.ToInt16(IFormatProvider provider)
            {
                return getvalue();
            }

            int IConvertible.ToInt32(IFormatProvider provider)
            {
                return getvalue();
            }

            long IConvertible.ToInt64(IFormatProvider provider)
            {
                return getvalue();
            }

            sbyte IConvertible.ToSByte(IFormatProvider provider)
            {
                return Convert.ToSByte(getvalue());
            }

            float IConvertible.ToSingle(IFormatProvider provider)
            {
                return getvalue();
            }

            string IConvertible.ToString(IFormatProvider provider)
            {
                if (value == true) return "true";
                return "false";
            }

            object IConvertible.ToType(Type conversionType, IFormatProvider provider)
            {
                return getvalue();
            }

            ushort IConvertible.ToUInt16(IFormatProvider provider)
            {
                return getvalue();
            }

            uint IConvertible.ToUInt32(IFormatProvider provider)
            {
                return getvalue();
            }

            ulong IConvertible.ToUInt64(IFormatProvider provider)
            {
                return getvalue();
            }

            public bool Equals(bool a)
            {
                return value;
            }

        }


        void tst()
        {
           //Bit a = false;
           // bool b = a;
        }



    }
}
