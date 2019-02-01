using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Ports;

using System.Threading;

namespace Eshiba
{
    /// <summary>
    /// Работа с COM портом
    /// </summary>
    class Serial
    {
        SerialPort port;
        int Speed = 9600;
        string port_nomber = "COM";

        /// <summary>
        /// Работа с COM портом
        /// </summary>
        /// <param name="str">Номер порта</param>
        public Serial(string str)
        {
            port_nomber = port_nomber + str;
            port = new SerialPort(port_nomber, Speed, Parity.None, 8, StopBits.One);
            port.Open();
        }

        /// <summary>
        /// Работа с COM портом
        /// </summary>
        /// <param name="str">Номер порта</param>
        public Serial(int str)
        {
            port_nomber = port_nomber + str.ToString();
            port = new SerialPort(port_nomber, Speed, Parity.None, 8, StopBits.One);
            port.Open();
        }

        /// <summary>
        /// Читает int32 из порта
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            pause();
            byte[] arr = new byte[4];
            port.Read(arr, 0, 4);
            return DataConverter.Byte4toInt32(arr);           
        }

        /// <summary>
        /// Читает int16 из порта
        /// </summary>
        /// <returns></returns>
        public int ReadInt16()
        {
            pause();
            byte[] arr = new byte[2];
            port.Read(arr, 0, 2);
            return DataConverter.Byte2ToInt16(arr);
        }

        /// <summary>
        /// Пишет Int32
        /// 2 в порт
        /// </summary>
        /// <param name="var"></param>
        public void WriteInt32(int var)
        {
            pause();
            byte[] arr = new byte[4];
            arr = DataConverter.IntTo4Byte(var);
            port.Write(arr, 0, 4);
        }

        /// <summary>
        /// Пишет Int16
        /// 2 в порт
        /// </summary>
        /// <param name="var"></param>
        public void WriteInt16(ushort var)
        {
            pause();
            byte[] arr = new byte[2];
            arr = DataConverter.Int16To2Byte(var);
            port.Write(arr, 0, 2);
        }

        public void WriteInt32Array(int[]buffer)
        {
            int length = buffer.Length;

            for (int i = 0; i < length; i++)
            {
                byte[] arr = DataConverter.IntTo4Byte(buffer[i]);
                port.Write(arr, 0, 4);
                Thread.Sleep(11);
            }
        }

        public int[] ReadInt32Array(int lenght)
        {
            int[] buffer = new int[lenght];
            for (int i = 0; i < lenght; i++)
            {
                byte[] arr = new byte[4];
                port.Read(arr, 0, 4);
                buffer[i] = DataConverter.Byte4toInt32(arr);
                Thread.Sleep(11);
            }
            return buffer;
        }

        /// <summary>
        /// Установить скрость
        /// </summary>
        /// <param name="spd"></param>
        public void SetSpeed(int spd)
        {
            try
            {
                port.Close();
            }
            catch { }
            Speed = spd;
            port = new SerialPort(port_nomber, Speed, Parity.None, 8, StopBits.One);
        }

        /// <summary>
        /// Закрыть порт
        /// </summary>
        public void Close()
        {
            try
            {
                port.Close();
            }
            catch { }
        }

        /// <summary>
        /// Открыть порт
        /// </summary>
        public void Open()
        {
            try
            {
                port.Close();
            }
            catch { }
            port = new SerialPort(port_nomber, Speed, Parity.None, 8, StopBits.One);
            port.Open();
        }

        void pause()
        {
            Thread.Sleep(11);
        }

    }
}
