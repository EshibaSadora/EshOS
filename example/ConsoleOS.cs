using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Esh.ConsoleOS;

using System.Runtime.InteropServices;

using System.Reflection;

namespace example_consoleOS
{
    class Program
    {
        static void Main(string[] args)
        {
            //Forms
             string lbl = "Это то что под заголовком";
             string[] info = {"Описание кнопки1", "Описание кнопки2"};
             string[] btns = { "Кнопка1", "Кнопка2" };

             int[] min = { 10, 50 };
             int[] max = { 25, 100 };

             OSLog.Log("Нажата кнопка: " + OS.form(btns, "Заголовок1", lbl, info));
             OS.AnyKey();

             OSLog.Log("Вы ввели: " + OS.InputForm("Введите что то", "Тут описание"));
             OS.AnyKey();


             int [] dt = OS.NumericForm(lbl,btns,info,min,max);

             for(int i = 0; i < btns.Length;i++)
             {
                 Console.WriteLine("Значение " + (i+1).ToString() + " параметра = " + dt[i]);
             }
             
              
			 //find all folder files
             string [] str = OS.DirFiles();

			 //compiller c#
             Esh.Compiller.Compiller.Make(false, "tst.cs");

             OS.AnyKey();
        }
    }
}
