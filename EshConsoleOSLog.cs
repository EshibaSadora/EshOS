using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esh.ConsoleOS
{
    public static class OSLog
    {
        public static  void Error(string a)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ошибка: " + a);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Log(string a)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Log: " + a);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Log(int a)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Log: " + a);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Log(float a)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Log: " + a);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Log(byte[] a, int first, int end)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("Log: ");
            for (int i = first; i < end; i++)
            {
                
                Console.Write(" ["+ a[i] + "]");
            }
            Console.WriteLine("EndLog.");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Msg(string a)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(a);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Msg(int a)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(a);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Msg(float a)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(a);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Msg(byte[] a, int first, int end)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Массив байтов:");

            for (int i = first; i < end; i++)
            {

                Console.Write(" [" + a[i] + "]");
            }
            Console.WriteLine("конец массива.");

            Console.ForegroundColor = ConsoleColor.White;
        }



    }
}
