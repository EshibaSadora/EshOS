//Версия 1.0.0

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System;

namespace Eshiba
{
    public class IniFile
    {
        string Path; //Имя файла.

        [DllImport("kernel32")] // Подключаем kernel32.dll и описываем его функцию WritePrivateProfilesString
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32")] // Еще раз подключаем kernel32.dll, а теперь описываем функцию GetPrivateProfileString
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        /// <summary>
        ///  С помощью конструктора записываем пусть до файла и его имя.
        /// </summary>
        /// <param name="IniPath"></param>
        public IniFile(string IniPath)
        {
            Path = new FileInfo(IniPath).FullName.ToString();
        }

        /// <summary>
        /// Читаем ini-файл и возвращаем значение указного ключа из заданной секции в формате String.
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string Read(string Section, string Key)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            return RetVal.ToString();
        }

        /// <summary>
        /// Считывает число с Экстремулой и конвертирует его в Int32
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public int ReadInt_E(string Section, string Key)
        {

            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            string RetString = RetVal.ToString();

            // RetString = RetString.Substring(0, 19);

            int RetInt = 0;
            float RetFloat = 0;
            if (RetString != "")
            {
                Single.TryParse(RetString, out RetFloat);
            }

            RetInt = Convert.ToInt32(RetFloat);



            return RetInt;
        }


        /// <summary>
        /// Считывает число с Экстремулой и конвертирует его в Float
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public float ReadFloat_E(string Section, string Key)
        {

            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            string RetString = RetVal.ToString();

            // RetString = RetString.Substring(0, 19);


            float RetFloat = 0;

            if (RetString != "")
            {
                Single.TryParse(RetString, out RetFloat);
            }

            return RetFloat;
        }


        /// <summary>
        /// Читаем ini-файл и возвращаем значение указного ключа из заданной секции в Int.
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public int ReadInt(string Section, string Key)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            string RetString = RetVal.ToString();

            int RetInt;
            if (RetString != "")
            {
                RetInt = Convert.ToInt32(RetString);
            }
            else { RetInt = 0; }
            return RetInt;
        }


        /// <summary>
        /// Читаем ini-файл и возвращаем значение указного ключа из заданной секции в Float.
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public float ReadFloat(string Section, string Key)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            string RetString = RetVal.ToString();
            int RetInt = 0;
            if (RetString != "")
            {
                RetInt = Convert.ToInt32(RetString);
            }
            float RetFloat = RetInt;
            return RetFloat;
        }
        /// <summary>
        /// Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Write(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        /// <summary>
        /// Удаляем ключ из выбранной секции.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        public void DeleteKey(string Key, string Section = null)
        {
            Write(Section, Key, null);
        }

        /// <summary>
        /// Удаляем выбранную секцию
        /// </summary>
        /// <param name="Section"></param>
        public void DeleteSection(string Section = null)
        {
            Write(Section, null, null);
        }

        /// <summary>
        /// Проверяем, есть ли такой ключ, в этой секции
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Section"></param>
        /// <returns></returns>
        public bool KeyExists(string Key, string Section = null)
        {
            return Read(Section, Key).Length > 0;
        }
    }

}