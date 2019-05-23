//Версия 1.0.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Eshiba.Forms;

namespace Eshiba.ConsoleOS
{
    static public class OS
    {
        static bool sist = false;

        static void menu(int pos, string label, string[] buttons, ConsoleColor col)
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

            for (int i = 0; i < buttons.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                if (pos == i) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
                Console.WriteLine(buttons[i]);
            }

        }

        static void numeric_menu(int pos, string[] buttons, int[] vars)
        {
            //Console.Clear();

            for (int i = 0; i < buttons.Length; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                if (pos == i) { Console.BackgroundColor = ConsoleColor.White; Console.ForegroundColor = ConsoleColor.Black; }
                Console.WriteLine(buttons[i] + " <" + vars[i] + ">");
            }

        }

        /// <summary>
        /// Рисует линию из "-" на весь экран
        /// </summary>
        static public void drawline()
        {
            // Console.WriteLine("");
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }
            //  Console.WriteLine("");
        }

        /// <summary>
        /// Рисует линию на весь экран заданным символом
        /// </summary>
        /// <param name="a">символ</param>
        static public void drawline(char a)
        {
            // Console.WriteLine("");
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write(a);
            }
            //  Console.WriteLine("");
        }

        static int horizontal_cursor(int menupoz, string[] buttons, int[] data, int poz)
        {
            int var = data[poz];

            ConsoleKeyInfo cki;
            cki = Console.ReadKey(true);

            if (cki.Key == ConsoleKey.LeftArrow) { var--; }
            if (cki.Key == ConsoleKey.RightArrow) { var++; }

            if (cki.Key == ConsoleKey.DownArrow) { menupoz++; }
            if (cki.Key == ConsoleKey.UpArrow) { menupoz--; }


            if (menupoz >= buttons.Length) { menupoz = 0; }
            if (menupoz < 0) { menupoz = 0; }

            if (cki.Key == ConsoleKey.Enter) { sist = true; }

            data[poz] = var;

            return menupoz;

        }


        static int horizontal_cursor(int menupoz, string[] buttons, int[] data, int poz, int[] min, int[] max)
        {
            int var = data[poz];

            ConsoleKeyInfo cki;
            cki = Console.ReadKey(true);

            if (cki.Key == ConsoleKey.LeftArrow) { var--; }
            if (var < min[poz]) { var = min[poz]; }
            if (var > max[poz]) { var = max[poz]; }

            if (cki.Key == ConsoleKey.RightArrow) { var++; }

            if (cki.Key == ConsoleKey.DownArrow) { menupoz++; }
            if (cki.Key == ConsoleKey.UpArrow) { menupoz--; }


            if (menupoz >= buttons.Length) { menupoz = 0; }
            if (menupoz < 0) { menupoz = 0; }

            if (cki.Key == ConsoleKey.Enter) { sist = true; }

            data[poz] = var;

            return menupoz;
        }

        static int cursor(int menupoz, string[] buttons)
        {
            ConsoleKeyInfo cki;


            cki = Console.ReadKey(true);

            if (cki.Key == ConsoleKey.DownArrow) { menupoz++; }
            if (cki.Key == ConsoleKey.UpArrow) { menupoz--; }


            if (menupoz >= buttons.Length) { menupoz = 0; }
            if (menupoz < 0) { menupoz = 0; }

            if (cki.Key == ConsoleKey.Enter) { sist = true; }

            //  Msg.Show(menupoz);

            Console.Clear();

            return menupoz;

        }



        /// <summary>
        /// Создаёт консольную форму, выбора вариантов
        /// </summary>
        /// <param name="buttons">Массив из кнопок ("Кнопка1","Кнопка2"...)</param>
        /// <param name="mess">Собщение под заголовком</param>
        /// <param name="info">Текст описания кнопки</param>
        /// <returns></returns>
        static public int form(string[] buttons, string mess, string[] info)
        {

            bool switcher = false;
            int pos = 0;

            while (switcher == false)
            {
                Console.Clear();
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
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(info[pos]);
                drawline('*');
                pos = cursor(pos, buttons);

                if (sist == true) { Console.Clear(); sist = false; return pos + 1; };
            }

            Console.Clear();
            return -1;
        }


        static public int form(string[] buttons)
        {

            bool switcher = false;
            int pos = 0;

            while (switcher == false)
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');

                Console.ForegroundColor = ConsoleColor.White;


                menu(pos, buttons);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                pos = cursor(pos, buttons);

                if (sist == true) { Console.Clear(); sist = false; return pos + 1; };
            }

            Console.Clear();
            return -1;
        }


        /// <summary>
        /// шапка формы
        /// </summary>
        /// <param name="label"></param>
        static public void Head(string label)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            drawline('*');
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(label);
            Console.ForegroundColor = ConsoleColor.Cyan;
            drawline('*');
        }

        /// <summary>
        /// Вызывает ожидание системы
        /// </summary>
        static public void AnyKey()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Нажмите любую кнопку");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        static public string InputForm(string label, string text)
        {

            Console.Clear();
            string str;
            Head(label);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
            Console.WriteLine();

            str = Console.ReadLine();
            return str;
        }

        /// <summary>
        /// Множество числовых значений с описаниями, который пользователь может меняьб
        /// </summary>
        static public int[] NumericForm(string label, string[] buttons, string[] info)
        {
            int[] data = new int[buttons.Length];

            bool switcher = false;
            int pos = 0;

            while (switcher == false)
            {
                Console.Clear();
                Head(label);

                numeric_menu(pos, buttons, data);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(info[pos]);
                drawline('*');
                // pos = cursor(pos, buttons);
                pos = horizontal_cursor(pos, buttons, data, pos);
                Console.Clear();

                if (sist == true) { sist = false; return data; };
            }
            return data;
        }

        /// <summary>
        /// Множество числовых значений с описаниями, который пользователь может меняьб
        /// </summary>
        static public int[] NumericForm(string label, string[] buttons, string[] info, int[] min, int[] max)
        {
            int[] data = new int[buttons.Length];

            bool switcher = false;
            int pos = 0;



            while (switcher == false)
            {
                Console.Clear();
                Head(label);

                numeric_menu(pos, buttons, data);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                drawline('*');
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(info[pos]);
                drawline('*');
                // pos = cursor(pos, buttons);
                pos = horizontal_cursor(pos, buttons, data, pos, min, max);
                Console.Clear();

                if (sist == true) { sist = false; return data; };
            }
            return data;
        }

        static public string[] DirFiles(string path )
        {
            

            var dir = new DirectoryInfo(path); // папка с файлами 

            string[] str = new string[dir.GetFiles().Length];

            int i = 0;
            foreach (FileInfo file in dir.GetFiles())
            {
                str[i] = file.FullName;
                i++;
            }

            return str;
        }

        static public string[] DirFiles()
        {


            var dir = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory()); // папка с файлами 

            string[] str = new string[dir.GetFiles().Length];

            int i = 0;
            foreach (FileInfo file in dir.GetFiles())
            {
                str[i] = Path.GetFileNameWithoutExtension(file.FullName);
                i++;
            }

            return str;
        }

    }
}
