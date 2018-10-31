using System;
using System.Windows.Forms;
using System.IO;

namespace Esh.Forms
{
    public class Filedialog
    {
        /// <summary>
        /// Возвращает строковое значение пути до файла, который нужно загрузить
        /// <param name="filetype">filetype - Укажите расширение файла</param>
        /// </summary>
        public string openfile(string filetype)
        {
            string file = null;
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            openFileDialog1.Filter = filetype + " files (*." + filetype + ")|*." + filetype + "|All files (*.*)|*.*";
            // openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            //Код чтения
                            file = openFileDialog1.FileName;
                            myStream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно прочитать файл  " + ex.ToString());

                }
            }

            return file;
        }

        /// <summary>
        /// Возвращает строковое значение пути до файла, который нужно загрузить
        /// <param name="filetype">filetype - Укажите расширение файла</param>
        /// </summary>
        public string openfile()
        {
            string file = null;
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
           // openFileDialog1.Filter = filetype + " files (*." + filetype + ")|*." + filetype + "|All files (*.*)|*.*";
            // openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            //Код чтения
                            file = openFileDialog1.FileName;
                            myStream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно прочитать файл  " + ex.ToString());

                }
            }

            return file;
        }

        /// <summary>
        /// Возвращает строковое значение пути до файла, который нужно сохранить.
        /// Создаёт пустой файл.
        /// <param name="filetype">filetype - Укажите расширение файла</param>
        /// </summary>
        public string savefile(string filetype)
        {
            string dir = null;

            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = filetype + " files (*." + filetype + ")|*." + filetype + "|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    dir = saveFileDialog1.FileName;

                    myStream.Close();
                }
            }

            return dir;
        }
    }

    public static class Msg
    {
        public static void Show()
        {
            MessageBox.Show("Тут");
        }

        public static void Show(int a)
        {
            MessageBox.Show(a.ToString());
        }

        public static void Show(string a)
        {
            MessageBox.Show(a);
        }

        public static void Show(float a)
        {
            MessageBox.Show(a.ToString());
        }

        public static void Show(decimal a)
        {
            MessageBox.Show(a.ToString());
        }

    }
}

