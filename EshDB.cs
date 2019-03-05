using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Esiba
{

    class Database
    {
        eshdb db = new eshdb();
        string path = "";

        struct dbtabelcolumn
        {
            //0-256 byte - name
            //256 -  тип
            //257 - 260 - доп размер
            //261 - 264 - колличество элементов 

            public string Name;
            public byte[] value;
            /// <summary>
            /// 0 - byte, 1 - int16, 2 - int32, 3 - int64, 4 - string(256 byte), 5 - addition
            /// </summary>
            public byte type;
            public int addlenghth;

            public int count;//колличество згачений в колонке
        }
        struct dbtabel
        {
            //0-256 byte - name
            //256-259 - count columns   

            public string Name;
            public dbtabelcolumn[] Columns;
        }

        struct eshdb
        {
            //0-255 byte - name
            //256-259 - count tabels     

            public string Name;
            public dbtabel[] Tabels;
        }


        byte[] data = new byte[0];

        public Database(string _path)
        {
            path = _path;
            try
            {
                BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open));
                br.Close();
                LoadDb();
            }
            catch
            {
                //CreateDb();
            }
        }

        void SaveDb()
        {
            BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create));

            char[] name = new char[256];
            bw.Write(Eshiba.DataConverter.StrToChar(db.Name, 256));
            bw.Write(db.Tabels.Length);
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                bw.Write(Eshiba.DataConverter.StrToChar(db.Tabels[t].Name, 256));
                bw.Write(db.Tabels[t].Columns.Length);

                for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                {
                    bw.Write(Eshiba.DataConverter.StrToChar(db.Tabels[t].Columns[c].Name, 256));
                    bw.Write(db.Tabels[t].Columns[c].type);
                    bw.Write(db.Tabels[t].Columns[c].addlenghth);
                    bw.Write(db.Tabels[t].Columns[c].count);
                    bw.Write(db.Tabels[t].Columns[c].value);
                }
            }
            bw.Close();
        }

        void LoadDb()
        {
            BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open));
            db = new eshdb();
            db.Name = Eshiba.DataConverter.CharToStr(br.ReadChars(256));
            db.Tabels = new dbtabel[br.ReadInt32()];
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                db.Tabels[t].Name = Eshiba.DataConverter.CharToStr(br.ReadChars(256));
                db.Tabels[t].Columns = new dbtabelcolumn[br.ReadInt32()];

                for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                {
                    db.Tabels[t].Columns[c].Name = Eshiba.DataConverter.CharToStr(br.ReadChars(256));
                    db.Tabels[t].Columns[c].type = br.ReadByte();
                    db.Tabels[t].Columns[c].addlenghth = br.ReadInt32();
                    db.Tabels[t].Columns[c].count = br.ReadInt32();

                    switch (db.Tabels[t].Columns[c].type)
                    {
                        case 0:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].Columns[c].count);
                            break;
                        case 1:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].Columns[c].count * 2);
                            break;
                        case 2:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].Columns[c].count * 4);
                            break;
                        case 3:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].Columns[c].count * 8);
                            break;
                        case 4:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].Columns[c].count * 256);
                            break;
                        case 5:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].Columns[c].count * db.Tabels[t].Columns[c].addlenghth);
                            break;
                    }
                }
            }


            br.Close();
        }
        public void CreateDb(string name)
        {
            db = new eshdb();
            db.Name = Path.GetFileNameWithoutExtension(path);
            db.Tabels = new dbtabel[0];

        }

        public void CreateTabel(string name)
        {
            int lngth = db.Tabels.Length;
            Array.Resize(ref db.Tabels, lngth + 1);
            db.Tabels[lngth].Name = name;
            db.Tabels[lngth].Columns = new dbtabelcolumn[0];
        }

        /// <summary>
        /// Создать колонку
        /// </summary>
        /// <param name="name">Имя колонки</param>
        /// <param name="tabel">Таблица</param>
        /// <param name="type">0 - byte, 1 - int16, 2 - int32, 3 - int64, 4 - string(256 byte)</param>
        public void CreateColumn(string name, string tabel, byte type)
        {
            int id = -1;
            for(int i = 0; i < db.Tabels.Length; i++)
            {
                if (db.Tabels[i].Name == tabel) id = i;
            }
            if(id != -1)
            {
                int lngth = db.Tabels[id].Columns.Length;
                Array.Resize(ref db.Tabels[id].Columns, lngth + 1);

                db.Tabels[id].Columns[lngth].Name = name;
                db.Tabels[id].Columns[lngth].type = type;
                db.Tabels[id].Columns[lngth].count = 0;
            }
        }

        /// <summary>
        /// Создать колонку, с нестандартным размером ячейки
        /// </summary>
        /// <param name="name">Имя колонки</param>
        /// <param name="tabel">Таблица</param>
        /// <param name="addsize">размер ячейки в байтах</param>
        public void CreateColumnObject(string name, string tabel, int addsize)
        {
            int id = -1;
            for (int i = 0; i < db.Tabels.Length; i++)
            {
                if (db.Tabels[i].Name == tabel) id = i;
            }
            if (id != -1)
            {
                int lngth = db.Tabels[id].Columns.Length;
                Array.Resize(ref db.Tabels[id].Columns, lngth + 1);

                db.Tabels[id].Columns[lngth].Name = name;
                db.Tabels[id].Columns[lngth].type = 5;
                db.Tabels[id].Columns[lngth].addlenghth = addsize;
                db.Tabels[id].Columns[lngth].count = 0;
            }
        }

        public void AddValue(string tabel, string column, object var)
        {
            switch (findtype(column))
            {
                case 0:
                    AddByteValue(tabel, column, Convert.ToByte(var));
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }

        public void AddByteValue(string tabel, string column, byte var)
        {

        }

        public void AddInt16tValue(string tabel, string column, Int16 var)
        {

        }

        public void AddInt32Value(string tabel, string column, Int32 var)
        {

        }

        public void AddStrValue(string tabel, string column, string var)
        {

        }

        public void AddObjValue(string tabel, string column, byte[] var)
        {

        }

        /// <summary>
        /// 0 - byte, 1 - int16, 2 - int32, 3 - int64, 4 - string(256 byte), 5 - addition
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        byte findtype(string column)
        {
            for(int t = 0; t < db.Tabels.Length; t++)
            {
                for(int c =0; c < db.Tabels[t].Columns.Length;c++)
                {
                    if (db.Tabels[t].Columns[c].Name == column) return db.Tabels[t].Columns[c].type;
                }
            }
            return 0xFF;
        }


    }
}
