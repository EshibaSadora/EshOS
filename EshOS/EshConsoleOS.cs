using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esh.ConsoleOS
{
    static public class OS
    {
        static bool sist = false;

        static void menu(int pos,string label,string [] buttons,ConsoleColor col)
        {
            //Console.Clear();

            Console.WriteLine(label);

            for (int i = 1; i < buttons.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                if (pos == i) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
                Console.WriteLine(buttons[i]);
            }


          //  Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
        }
        static void menu(int pos, string[] buttons, ConsoleColor col)
        {
            //Console.Clear();

            for (int i = 1; i < buttons.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                if (pos == i) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
                Console.WriteLine(buttons[i]);
            }


            //  Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
        }
        static void menu(int pos, string[] buttons)
        {
            //Console.Clear();

            for (int i = 1; i < buttons.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                if (pos == i) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
                Console.WriteLine(buttons[i-1]);
            }
              
        }

        static public void drawline()
        {
            // Console.WriteLine("");
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }
            //  Console.WriteLine("");
        }

        static public void drawline(char a)
        {
            // Console.WriteLine("");
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write(a);
            }
            //  Console.WriteLine("");
        }

        static int cursor(int menupoz, string[] buttons)
        {
            ConsoleKeyInfo cki;


            cki = Console.ReadKey(true);

            if (cki.Key == ConsoleKey.DownArrow) { menupoz++; }
            if (cki.Key == ConsoleKey.UpArrow) { menupoz--; }


            if (menupoz > buttons.Length) { menupoz = 1; }
            if (menupoz < 1) { menupoz = 1; }

            if (cki.Key == ConsoleKey.Enter) { sist = true; }



            Console.Clear();
            return menupoz;

        }

        static public int form(string[] buttons)
        {
         //   string lbl = "Новая форма";
            string mess = "";

            bool loggin = false;
            int pos = 1;

            while (loggin == false)
            {
                Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(mess);
                Console.WriteLine();
                menu(pos, buttons);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');
                pos = cursor(pos, buttons);
                if (pos == buttons.Length + 2) { return pos-2; };
            }

            return -1;
        }
        static public int form(string[] buttons,string _lbl)
        {
            string lbl = _lbl;
            string mess = "";

            bool loggin = false;
            int pos = 1;

            while (loggin == false)
            {
                Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                // buttons = new string[] { "Вход", "Регистрация", "" };
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(mess);
                Console.WriteLine();
                menu(pos, buttons);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');
                pos = cursor(pos, buttons);
                if (pos == buttons.Length + 2) { return pos - 2; };
            }

            return -1;
        }
        static public int form(string[] buttons, string _lbl, string _mess)
        {
            string lbl = _lbl;
            string mess = _mess;

            bool loggin = false;
            int pos = 1;

            while (loggin == false)
            {
                Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                // buttons = new string[] { "Вход", "Регистрация", "" };
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(mess);
                Console.WriteLine();
                menu(pos, buttons);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');
                pos = cursor(pos, buttons);
                if (sist == true) { sist = false;  return pos; };
            }

            return -1;
        }

        static public void AnyKey()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Нажмите любую кнопку");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }
    }
}
