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

        public static void Msg(string a)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Log: " + a);
            Console.ForegroundColor = ConsoleColor.White;
        }



    }
}
