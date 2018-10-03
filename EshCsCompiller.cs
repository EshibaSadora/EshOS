using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;

using Esh.ConsoleOS;


namespace Esh.Compiller
{
    public static class Compiller
    {
        public static void Make(bool make_exe, string main, string[] dir_libs)
        {
            string source = "";
            //Грузим Main
            try
            {
                using (StreamReader sr = new StreamReader(main))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        source += line + "\n";
                    }
                }
            }


            catch (Exception e)
            {
                Console.WriteLine("Файл не может быть прочитан:");
                Console.WriteLine(e.Message);
            }

            //грузим libs
            string[] libs = OS.DirFiles();

            for (int i = 0; i < libs.Length; i++)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(libs[i]))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            source += line + "\n";
                        }
                    }
                }


                catch (Exception e)
                {
                    Console.WriteLine("Файл не может быть прочитан:");
                    Console.WriteLine(e.Message);
                }
            }

            // Настройки компиляции
            Dictionary<string, string> providerOptions = new Dictionary<string, string>();
            providerOptions.Add("CompilerVersion", "v2.0");

            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);
            CompilerParameters compilerParams = new CompilerParameters();

            switch (make_exe)
            {
            case true:
            compilerParams.OutputAssembly = "compile.exe";
            compilerParams.GenerateExecutable = true;
            break;
            case false:
            compilerParams.OutputAssembly = "compill.dll";
            compilerParams.GenerateExecutable = true;
            break;
            }

            // Компиляция
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, source);

            // Выводим информацию об ошибках
            Console.WriteLine("Number of Errors: {0}", results.Errors.Count);
            foreach (CompilerError err in results.Errors)
            {
                Console.WriteLine("ERROR {0}", err.ErrorText);
            }

        }


        public static void Make(bool make_exe, string main)
        {
            string source = "";
            //Грузим Main
            try
            {
                using (StreamReader sr = new StreamReader(main))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        source += line + "\n";
                    }
                }
            }


            catch (Exception e)
            {
                Console.WriteLine("Файл не может быть прочитан:");
                Console.WriteLine(e.Message);
            }

            // Настройки компиляции
            Dictionary<string, string> providerOptions = new Dictionary<string, string>();
            providerOptions.Add("CompilerVersion", "v2.0");

            CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);
            CompilerParameters compilerParams = new CompilerParameters();

            switch (make_exe)
            {
                case true:
                    compilerParams.OutputAssembly = "compill.exe";
                    compilerParams.GenerateExecutable = true;
                    break;
                case false:
                    compilerParams.OutputAssembly = "compill.dll";
                    compilerParams.GenerateExecutable = true;
                    break;
            }

            // Компиляция
            CompilerResults results = provider.CompileAssemblyFromSource(compilerParams, source);

            // Выводим информацию об ошибках
            Console.WriteLine("Number of Errors: {0}", results.Errors.Count);
            foreach (CompilerError err in results.Errors)
            {
                Console.WriteLine("ERROR {0}", err.ErrorText);
            }

        }
    }
}
