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

           
        }
        struct dbtabel
        {
            //0-256 byte - name
            //256-259 - count columns   

            public string Name;
            public dbtabelcolumn[] Columns;
            public int count;//колличество згачений в колонке
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
                Load();
            }
            catch
            {
                //CreateDb();
            }
        }

        void Save()
        {
            BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.Create));

            char[] name = new char[256];
            bw.Write(Eshiba.DataConverter.StrToChar(db.Name, 256));
            bw.Write(db.Tabels.Length);
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                bw.Write(Eshiba.DataConverter.StrToChar(db.Tabels[t].Name, 256));
                bw.Write(db.Tabels[t].Columns.Length);
                bw.Write(db.Tabels[t].count);

                for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                {
                    bw.Write(Eshiba.DataConverter.StrToChar(db.Tabels[t].Columns[c].Name, 256));
                    bw.Write(db.Tabels[t].Columns[c].type);
                    bw.Write(db.Tabels[t].Columns[c].addlenghth);
                    bw.Write(db.Tabels[t].Columns[c].value);
                }
            }
            bw.Close();
        }

        void Load()
        {
            BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open));
            db = new eshdb();
            db.Name = Eshiba.DataConverter.CharToStr(br.ReadChars(256));
            db.Tabels = new dbtabel[br.ReadInt32()];
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                db.Tabels[t].Name = Eshiba.DataConverter.CharToStr(br.ReadChars(256));
                db.Tabels[t].Columns = new dbtabelcolumn[br.ReadInt32()];
                db.Tabels[t].count = br.ReadInt32();

                for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                {
                    db.Tabels[t].Columns[c].Name = Eshiba.DataConverter.CharToStr(br.ReadChars(256));
                    db.Tabels[t].Columns[c].type = br.ReadByte();
                    db.Tabels[t].Columns[c].addlenghth = br.ReadInt32();

                    switch (db.Tabels[t].Columns[c].type)
                    {
                        case 0:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].count);
                            break;
                        case 1:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].count * 2);
                            break;
                        case 2:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].count * 4);
                            break;
                        case 3:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].count * 8);
                            break;
                        case 4:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].count * 256);
                            break;
                        case 5:
                            db.Tabels[t].Columns[c].value = br.ReadBytes(db.Tabels[t].count * db.Tabels[t].Columns[c].addlenghth);
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
            db.Tabels[lngth].count = 0;
        }

        /// <summary>
        /// Создать колонку
        /// </summary>
        /// <param name="name">Имя колонки</param>
        /// <param name="tabel">Таблица</param>
        /// <param name="type">0 - byte, 1 - int16, 2 - int32, 3 - int64, 4 - string(256 byte)</param>
        public void CreateColumn(string tabel, string name, byte type)
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

                switch (type)
                {
                    case 0: Array.Resize(ref db.Tabels[id].Columns[lngth].value, db.Tabels[id].count);break;
                    case 1: Array.Resize(ref db.Tabels[id].Columns[lngth].value, db.Tabels[id].count * 2); break;
                    case 2: Array.Resize(ref db.Tabels[id].Columns[lngth].value, db.Tabels[id].count * 4); break;
                    case 3: Array.Resize(ref db.Tabels[id].Columns[lngth].value, db.Tabels[id].count * 8); break;
                    case 4: Array.Resize(ref db.Tabels[id].Columns[lngth].value, db.Tabels[id].count * 256); break;
                }
                
            }
        }

        /// <summary>
        /// Создать колонку, с нестандартным размером ячейки
        /// </summary>
        /// <param name="name">Имя колонки</param>
        /// <param name="tabel">Таблица</param>
        /// <param name="addsize">размер ячейки в байтах</param>
        public void CreateColumnObject(string tabel, string name, int addsize)
        {
            int id = -1;
            for (int i = 0; i < db.Tabels.Length; i++)
            {
                if (db.Tabels[i].Name == tabel) id = i;
            }
            if (id != -1)
            {
                int lngth = db.Tabels[id].Columns.Length;

                if (db.Tabels[id].count <= db.Tabels[id].Columns[lngth].value.Length / db.Tabels[id].Columns[lngth].addlenghth) GrowTable(id);

                db.Tabels[id].Columns[lngth].Name = name;
                db.Tabels[id].Columns[lngth].type = 5;
                db.Tabels[id].Columns[lngth].addlenghth = addsize;

                 Array.Resize(ref db.Tabels[id].Columns[lngth].value, db.Tabels[id].count * addsize);

            }
        }

        /// <summary>
        /// Добавляет значение (работает с byte,int)
        /// </summary>
        /// <param name="tabel">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="var">Значение</param>
        public void AddValue(string tabel, string column, object var)
        {
            switch (findtype(column))
            {
                case 0:
                    AddByteValue(tabel, column, Convert.ToByte(var));
                    break;
                case 1:
                    AddInt16tValue(tabel, column, Convert.ToInt16(var));
                    break;
                case 2:
                    AddInt32Value(tabel, column, Convert.ToInt32(var));
                    break;
                case 3:
                    AddInt64Value(tabel, column, Convert.ToInt64(var));
                    break;
            }
        }

        /// <summary>
        /// Увеличивает размеры таблицы на 1
        /// </summary>
        void GrowTable(int id)
        {
            db.Tabels[id].count++;
            for (int i = 0; i < db.Tabels[id].Columns.Length; i++)
            {
                if (db.Tabels[id].Columns[i].type == 0) Array.Resize(ref db.Tabels[id].Columns[i].value, db.Tabels[id].count);
                if (db.Tabels[id].Columns[i].type == 1) Array.Resize(ref db.Tabels[id].Columns[i].value, db.Tabels[id].count * 2);
                if (db.Tabels[id].Columns[i].type == 2) Array.Resize(ref db.Tabels[id].Columns[i].value, db.Tabels[id].count * 4);
                if (db.Tabels[id].Columns[i].type == 3) Array.Resize(ref db.Tabels[id].Columns[i].value, db.Tabels[id].count * 8);
                if (db.Tabels[id].Columns[i].type == 4) Array.Resize(ref db.Tabels[id].Columns[i].value, db.Tabels[id].count * 256);
                if (db.Tabels[id].Columns[i].type == 5) Array.Resize(ref db.Tabels[id].Columns[i].value, db.Tabels[id].count * db.Tabels[id].Columns[i].addlenghth);

            }
        }

        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// <param name="var">Байтовое значение</param>
        public void AddByteValue(string tabel, string column, byte var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            int pos = db.Tabels[t].count;
                            if (db.Tabels[t].Columns[c].value ==  null) GrowTable(t); else
                            if (db.Tabels[t].count <= db.Tabels[t].Columns[c].value.Length) GrowTable(t);
                            db.Tabels[t].Columns[c].value[pos] = var;
                        }
                    }
                }
            }
        }



        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// <param name="var">Int16 значение</param>
        public void AddInt16tValue(string tabel, string column, Int16 var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            int pos = db.Tabels[t].count * 2;
                            if (db.Tabels[t].Columns[c].value == null)  GrowTable(t);
                            else
                            if (db.Tabels[t].count <= db.Tabels[t].Columns[c].value.Length / 2) GrowTable(t);
                            byte [] arr = BitConverter.GetBytes(var);
                            db.Tabels[t].Columns[c].value[pos] = arr[0];
                            db.Tabels[t].Columns[c].value[pos+1] = arr[1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// <param name="var">Int32 значение</param>
        public void AddInt32Value(string tabel, string column, Int32 var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            int pos = db.Tabels[t].count * 4;
                            if (db.Tabels[t].Columns[c].value == null) GrowTable(t);
                            else
                            if (db.Tabels[t].count <= db.Tabels[t].Columns[c].value.Length / 4) GrowTable(t);
                            byte[] arr = BitConverter.GetBytes(var);
                            db.Tabels[t].Columns[c].value[pos] = arr[0];
                            db.Tabels[t].Columns[c].value[pos + 1] = arr[1];
                            db.Tabels[t].Columns[c].value[pos + 2] = arr[2];
                            db.Tabels[t].Columns[c].value[pos + 3] = arr[3];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// <param name="var">Int64 значение</param>
        public void AddInt64Value(string tabel, string column, Int64 var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            int pos = db.Tabels[t].count * 8;
                            if (db.Tabels[t].Columns[c].value == null) GrowTable(t);
                            else
                            if (db.Tabels[t].count <= db.Tabels[t].Columns[c].value.Length / 8) GrowTable(t);
                            byte[] arr = BitConverter.GetBytes(var);
                            db.Tabels[t].Columns[c].value[pos] = arr[0];
                            db.Tabels[t].Columns[c].value[pos + 1] = arr[1];
                            db.Tabels[t].Columns[c].value[pos + 2] = arr[2];
                            db.Tabels[t].Columns[c].value[pos + 3] = arr[3];
                            db.Tabels[t].Columns[c].value[pos + 4] = arr[4];
                            db.Tabels[t].Columns[c].value[pos + 5] = arr[5];
                            db.Tabels[t].Columns[c].value[pos + 6] = arr[6];
                            db.Tabels[t].Columns[c].value[pos + 7] = arr[7];

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// <param name="var">string значение</param>
        public void AddStrValue(string tabel, string column, string var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            int pos = db.Tabels[t].count * 256;
                            if (db.Tabels[t].Columns[c].value == null) GrowTable(t);
                            else
                            if (db.Tabels[t].count <= db.Tabels[t].Columns[c].value.Length / 256) GrowTable(t);
                            byte[] arr = Eshiba.DataConverter.StrToByte(var, 128);
                            for(int i = 0; i < 256; i++)
                            {
                                db.Tabels[t].Columns[c].value[pos + i]  = arr[i];
                            }                                                  
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Получает Byte
        /// </summary>
        /// <param name="tabel">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="id">Номер строки</param>
        /// <returns></returns>
        public byte GetByteValue(string tabel, string column, int id)
        {
            byte var = 0x00;
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            var = db.Tabels[t].Columns[c].value[id];
                        }
                    }
                }
            }
            return var;
        }


        /// <summary>
        /// Получает Int16
        /// </summary>
        /// <param name="tabel">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="id">Номер строки</param>
        /// <returns></returns>
        public Int16 GetInt16Value(string tabel, string column, int id)
        {
            Int16 var = 0;
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            byte[] arr = new byte[2];
                            arr[0] = db.Tabels[t].Columns[c].value[id*2];
                            arr[1] = db.Tabels[t].Columns[c].value[id * 2 + 1];
                            var = BitConverter.ToInt16(arr, 0);
                        }
                    }
                }
            }
            return var;
        }

        /// <summary>
        /// Получает Int32
        /// </summary>
        /// <param name="tabel">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="id">Номер строки</param>
        /// <returns></returns>
        public Int32 GetInt32Value(string tabel, string column, int id)
        {
            Int32 var = 0;
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            byte[] arr = new byte[4];
                            arr[0] = db.Tabels[t].Columns[c].value[id * 4];
                            arr[1] = db.Tabels[t].Columns[c].value[id * 4 + 1];
                            arr[2] = db.Tabels[t].Columns[c].value[id * 4 + 2];
                            arr[3] = db.Tabels[t].Columns[c].value[id * 4 + 3];
                            var = BitConverter.ToInt32(arr, 0);
                        }
                    }
                }
            }
            return var;
        }

        /// <summary>
        /// Получает Int64
        /// </summary>
        /// <param name="tabel">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="id">Номер строки</param>
        /// <returns></returns>
        public Int64 GetInt64Value(string tabel, string column, int id)
        {
            Int64 var = 0;
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            byte[] arr = new byte[8];
                            arr[0] = db.Tabels[t].Columns[c].value[id * 8];
                            arr[1] = db.Tabels[t].Columns[c].value[id * 8 + 1];
                            arr[2] = db.Tabels[t].Columns[c].value[id * 8 + 2];
                            arr[3] = db.Tabels[t].Columns[c].value[id * 8 + 3];
                            arr[4] = db.Tabels[t].Columns[c].value[id * 8 + 4];
                            arr[5] = db.Tabels[t].Columns[c].value[id * 8 + 5];
                            arr[6] = db.Tabels[t].Columns[c].value[id * 8 + 6];
                            arr[7] = db.Tabels[t].Columns[c].value[id * 8 + 7];
                            var = BitConverter.ToInt64(arr, 0);
                        }
                    }
                }
            }
            return var;
        }

        /// <summary>
        /// Получает строку 
        /// </summary>
        /// <param name="tabel">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="id">Номер строки</param>
        /// <returns></returns>
        public string GetStrValue(string tabel, string column, int id)
        {
            string str = "";
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            int pos = id * 256;

                            byte[] arr = new byte[256];

                            for (int i = 0; i < 256; i++)
                            {
                                arr[i] = db.Tabels[t].Columns[c].value[pos + i];
                            }

                            str = Eshiba.DataConverter.ByteToStr(arr, 256);
                        }
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// Добавить Объект
        /// </summary>
        /// <param name="tabel">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="var">значение</param>
        public void AddObjValue(string tabel, string column, byte[] var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            int pos = db.Tabels[t].count * db.Tabels[t].Columns[c].addlenghth;
                            if (db.Tabels[t].Columns[c].value == null) GrowTable(t);
                            else
                            if (db.Tabels[t].count <= db.Tabels[t].Columns[c].value.Length / db.Tabels[t].Columns[c].addlenghth) GrowTable(t);
                            for (int i = 0; i < db.Tabels[t].Columns[c].addlenghth; i++)
                            {
                                db.Tabels[t].Columns[c].value[pos + i] = var[i];
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="tabel">Таблица</param>
        /// <param name="column">Колонка</param>
        /// <param name="id">Номер строки</param>
        /// <returns></returns>
        public byte[] GetObjValue(string tabel, string column, int id)
        {
            byte[] var = new byte[0];
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            var = new byte[db.Tabels[t].Columns[c].addlenghth];

                            int pos = id * db.Tabels[t].Columns[c].addlenghth;

                            for (int i = 0; i < db.Tabels[t].Columns[c].addlenghth; i++)
                            {
                                var[i] = db.Tabels[t].Columns[c].value[pos + i];
                            }

                        }
                    }
                }
            }
            return var;
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

        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public byte[] GetByteWhere(string wheretabel, string column, string wherecolumn, byte value)
        {
          
            int[] id = new int[0];
            


            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for(int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].value[i] == value)
                                {
                                    int pos = id.Length;
                                    Array.Resize(ref id, pos + 1);
                                    id[pos] = i; 
                                }
                            }
                        }
                    }
                }
            }

            byte[] data = new byte[0];

            if (id.Length != 0)
            {
                data = new byte[id.Length];

                for(int i =0; i < id.Length; i++)
                {
                    data[i] = GetByteValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public byte[] GetByteWhere(string wheretabel, string column, string wherecolumn, Int16 value)
        {

            int[] id = new int[0];

            byte [] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 1)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 2] == val[0] & db.Tabels[t].Columns[c].value[i * 2 + 1] == val[1])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            byte[] data = new byte[0];

            if (id.Length != 0)
            {
                data = new byte[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetByteValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public byte[] GetByteWhere(string wheretabel, string column, string wherecolumn, Int32 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 2)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1] & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            byte[] data = new byte[0];

            if (id.Length != 0)
            {
                data = new byte[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetByteValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public byte[] GetByteWhere(string wheretabel, string column, string wherecolumn, Int64 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 3)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1] 
                                        & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3] 
                                        & db.Tabels[t].Columns[c].value[i * 4 + 4] == val[4] & db.Tabels[t].Columns[c].value[i * 4 + 5] == val[5]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 6] == val[6] & db.Tabels[t].Columns[c].value[i * 4 + 7] == val[7])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            byte[] data = new byte[0];

            if (id.Length != 0)
            {
                data = new byte[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetByteValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public byte[] GetByteWhere(string wheretabel, string column, string wherecolumn, string value)
        {

            int[] id = new int[0];

            byte[] val = Eshiba.DataConverter.StrToByte(value, 128);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 4)
                                {
                                    byte[] data0 = new byte[256];


                                    for(int ii =0; ii < 256; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * 256 + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0,val))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            byte[] data = new byte[0];

            if (id.Length != 0)
            {
                data = new byte[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetByteValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public byte[] GetByteWhere(string wheretabel, string column, string wherecolumn, byte [] value)
        {

            int[] id = new int[0];


            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 5)
                                {
                                    byte[] data0 = new byte[db.Tabels[t].Columns[c].addlenghth];


                                    for (int ii = 0; ii < db.Tabels[t].Columns[c].addlenghth; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * db.Tabels[t].Columns[c].addlenghth + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0, value))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            byte[] data = new byte[0];

            if (id.Length != 0)
            {
                data = new byte[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetByteValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int16[] GetInt16Where(string wheretabel, string column, string wherecolumn, byte value)
        {

            int[] id = new int[0];



            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].value[i] == value)
                                {
                                    int pos = id.Length;
                                    Array.Resize(ref id, pos + 1);
                                    id[pos] = i;
                                }
                            }
                        }
                    }
                }
            }

            Int16[] data = new Int16[0];

            if (id.Length != 0)
            {
                data = new Int16[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt16Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }



        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int16[] GetInt16Where(string wheretabel, string column, string wherecolumn, Int16 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 1)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 2] == val[0] & db.Tabels[t].Columns[c].value[i * 2 + 1] == val[1])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int16[] data = new Int16[0];

            if (id.Length != 0)
            {
                data = new Int16[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt16Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int16[] GetInt16Where(string wheretabel, string column, string wherecolumn, Int32 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 2)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1] & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int16[] data = new Int16[0];

            if (id.Length != 0)
            {
                data = new Int16[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt16Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }





        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int16[] GetInt16Where(string wheretabel, string column, string wherecolumn, Int64 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 3)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 4] == val[4] & db.Tabels[t].Columns[c].value[i * 4 + 5] == val[5]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 6] == val[6] & db.Tabels[t].Columns[c].value[i * 4 + 7] == val[7])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int16[] data = new Int16[0];

            if (id.Length != 0)
            {
                data = new Int16[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt16Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int16[] GetInt16Where(string wheretabel, string column, string wherecolumn, string value)
        {

            int[] id = new int[0];

            byte[] val = Eshiba.DataConverter.StrToByte(value, 128);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 4)
                                {
                                    byte[] data0 = new byte[256];


                                    for (int ii = 0; ii < 256; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * 256 + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0, val))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int16[] data = new Int16[0];

            if (id.Length != 0)
            {
                data = new Int16[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt16Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }

        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int16[] GetInt16Where(string wheretabel, string column, string wherecolumn, byte [] value)
        {

            int[] id = new int[0];

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 5)
                                {
                                    byte[] data0 = new byte[db.Tabels[t].Columns[c].addlenghth];


                                    for (int ii = 0; ii < db.Tabels[t].Columns[c].addlenghth; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * db.Tabels[t].Columns[c].addlenghth + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0, value))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int16[] data = new Int16[0];

            if (id.Length != 0)
            {
                data = new Int16[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt16Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }

        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int32[] GetInt32Where(string wheretabel, string column, string wherecolumn, byte value)
        {

            int[] id = new int[0];



            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].value[i] == value)
                                {
                                    int pos = id.Length;
                                    Array.Resize(ref id, pos + 1);
                                    id[pos] = i;
                                }
                            }
                        }
                    }
                }
            }

            Int32[] data = new Int32[0];

            if (id.Length != 0)
            {
                data = new Int32[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt32Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }



        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int32[] GetInt32Where(string wheretabel, string column, string wherecolumn, Int16 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 1)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 2] == val[0] & db.Tabels[t].Columns[c].value[i * 2 + 1] == val[1])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int32[] data = new Int32[0];

            if (id.Length != 0)
            {
                data = new Int32[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt32Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int32[] GetInt32Where(string wheretabel, string column, string wherecolumn, Int32 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 2)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1] & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int32[] data = new Int32[0];

            if (id.Length != 0)
            {
                data = new Int32[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt32Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }





        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int32[] GetInt32Where(string wheretabel, string column, string wherecolumn, Int64 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 3)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 4] == val[4] & db.Tabels[t].Columns[c].value[i * 4 + 5] == val[5]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 6] == val[6] & db.Tabels[t].Columns[c].value[i * 4 + 7] == val[7])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int32[] data = new Int32[0];

            if (id.Length != 0)
            {
                data = new Int32[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt32Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int32[] GetInt32Where(string wheretabel, string column, string wherecolumn, string value)
        {

            int[] id = new int[0];

            byte[] val = Eshiba.DataConverter.StrToByte(value, 128);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 4)
                                {
                                    byte[] data0 = new byte[256];


                                    for (int ii = 0; ii < 256; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * 256 + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0, val))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int32[] data = new Int32[0];

            if (id.Length != 0)
            {
                data = new Int32[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt32Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }

        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int32[] GetInt32Where(string wheretabel, string column, string wherecolumn, byte[] value)
        {

            int[] id = new int[0];

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 5)
                                {
                                    byte[] data0 = new byte[db.Tabels[t].Columns[c].addlenghth];


                                    for (int ii = 0; ii < db.Tabels[t].Columns[c].addlenghth; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * db.Tabels[t].Columns[c].addlenghth + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0, value))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int32[] data = new Int32[0];

            if (id.Length != 0)
            {
                data = new Int32[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt32Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }



        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int64[] GetInt64Where(string wheretabel, string column, string wherecolumn, byte value)
        {

            int[] id = new int[0];



            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].value[i] == value)
                                {
                                    int pos = id.Length;
                                    Array.Resize(ref id, pos + 1);
                                    id[pos] = i;
                                }
                            }
                        }
                    }
                }
            }

            Int64[] data = new Int64[0];

            if (id.Length != 0)
            {
                data = new Int64[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt64Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }



        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int64[] GetInt64Where(string wheretabel, string column, string wherecolumn, Int16 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 1)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 2] == val[0] & db.Tabels[t].Columns[c].value[i * 2 + 1] == val[1])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int64[] data = new Int64[0];

            if (id.Length != 0)
            {
                data = new Int64[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt64Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int64[] GetInt64Where(string wheretabel, string column, string wherecolumn, Int32 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 2)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1] & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int64[] data = new Int64[0];

            if (id.Length != 0)
            {
                data = new Int64[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt64Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }





        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int64[] GetInt64Where(string wheretabel, string column, string wherecolumn, Int64 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 3)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 4] == val[4] & db.Tabels[t].Columns[c].value[i * 4 + 5] == val[5]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 6] == val[6] & db.Tabels[t].Columns[c].value[i * 4 + 7] == val[7])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int64[] data = new Int64[0];

            if (id.Length != 0)
            {
                data = new Int64[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt64Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int64[] GetInt64Where(string wheretabel, string column, string wherecolumn, string value)
        {

            int[] id = new int[0];

            byte[] val = Eshiba.DataConverter.StrToByte(value, 128);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 4)
                                {
                                    byte[] data0 = new byte[256];


                                    for (int ii = 0; ii < 256; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * 256 + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0, val))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int64[] data = new Int64[0];

            if (id.Length != 0)
            {
                data = new Int64[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt64Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }

        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public Int64[] GetInt64Where(string wheretabel, string column, string wherecolumn, byte[] value)
        {

            int[] id = new int[0];

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 5)
                                {
                                    byte[] data0 = new byte[db.Tabels[t].Columns[c].addlenghth];


                                    for (int ii = 0; ii < db.Tabels[t].Columns[c].addlenghth; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * db.Tabels[t].Columns[c].addlenghth + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0, value))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Int64[] data = new Int64[0];

            if (id.Length != 0)
            {
                data = new Int64[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetInt64Value(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public string[] GetStrWhere(string wheretabel, string column, string wherecolumn, byte value)
        {

            int[] id = new int[0];

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].value[i] == value)
                                {
                                    int pos = id.Length;
                                    Array.Resize(ref id, pos + 1);
                                    id[pos] = i;
                                }
                            }
                        }
                    }
                }
            }

            string[] data = new string[0];

            if (id.Length != 0)
            {
                data = new string[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetStrValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }



        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public string[] GetStrWhere(string wheretabel, string column, string wherecolumn, Int16 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 1)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 2] == val[0] & db.Tabels[t].Columns[c].value[i * 2 + 1] == val[1])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string[] data = new string[0];

            if (id.Length != 0)
            {
                data = new string[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetStrValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public string[] GetStrWhere(string wheretabel, string column, string wherecolumn, Int32 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 2)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1] & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string[] data = new string[0];

            if (id.Length != 0)
            {
                data = new string[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetStrValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }





        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public string[] GetStrWhere(string wheretabel, string column, string wherecolumn, Int64 value)
        {

            int[] id = new int[0];

            byte[] val = BitConverter.GetBytes(value);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 3)
                                {
                                    if (db.Tabels[t].Columns[c].value[i * 4] == val[0] & db.Tabels[t].Columns[c].value[i * 4 + 1] == val[1]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 2] == val[2] & db.Tabels[t].Columns[c].value[i * 4 + 3] == val[3]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 4] == val[4] & db.Tabels[t].Columns[c].value[i * 4 + 5] == val[5]
                                        & db.Tabels[t].Columns[c].value[i * 4 + 6] == val[6] & db.Tabels[t].Columns[c].value[i * 4 + 7] == val[7])
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string[] data = new string[0];

            if (id.Length != 0)
            {
                data = new string[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetStrValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public string[] GetStr64Where(string wheretabel, string column, string wherecolumn, string value)
        {

            int[] id = new int[0];

            byte[] val = Eshiba.DataConverter.StrToByte(value, 128);

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 4)
                                {
                                    byte[] data0 = new byte[256];


                                    for (int ii = 0; ii < 256; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * 256 + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0, val))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string[] data = new string[0];

            if (id.Length != 0)
            {
                data = new string[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetStrValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }

        /// <summary>
        /// Получить значение по условию
        /// </summary>
        /// <param name="wheretabel">Таблица</param>
        /// <param name="column">Колонка с нужным значением</param>
        /// <param name="wherecolumn">Колонка по которой ищут</param>
        /// <param name="value">Значение по которому ищут</param>
        /// <returns></returns>
        public string[] GetStrWhere(string wheretabel, string column, string wherecolumn, byte[] value)
        {

            int[] id = new int[0];

            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == wheretabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == wherecolumn)
                        {
                            for (int i = 0; i < db.Tabels[t].count; i++)
                            {
                                if (db.Tabels[t].Columns[c].type == 5)
                                {
                                    byte[] data0 = new byte[db.Tabels[t].Columns[c].addlenghth];


                                    for (int ii = 0; ii < db.Tabels[t].Columns[c].addlenghth; ii++)
                                    {
                                        data0[ii] = db.Tabels[t].Columns[c].value[i * db.Tabels[t].Columns[c].addlenghth + ii];
                                    }

                                    if (Eshiba.Core.CpByteArr(data0, value))
                                    {
                                        int pos = id.Length;
                                        Array.Resize(ref id, pos + 1);
                                        id[pos] = i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            string[] data = new string[0];

            if (id.Length != 0)
            {
                data = new string[id.Length];

                for (int i = 0; i < id.Length; i++)
                {
                    data[i] = GetStrValue(wheretabel, column, id[i]);
                }
            }

            return data;
        }


        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// /// <param name="id">номер ячейки</param>
        /// <param name="var">Байтовое значение</param>
        public void SetByteValue(string tabel, string column, int id, byte var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            while(db.Tabels[t].count < id) GrowTable(t);
                            db.Tabels[t].Columns[c].value[id] = var;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// /// <param name="id">номер ячейки</param>
        /// <param name="var">Байтовое значение</param>
        public void SetInt16Value(string tabel, string column, int id, Int16 var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            while (db.Tabels[t].count < id) GrowTable(t);
                            byte[] arr = BitConverter.GetBytes(var);
                            db.Tabels[t].Columns[c].value[id * 2] = arr[0];
                            db.Tabels[t].Columns[c].value[id * 2 + 1] = arr[1];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// /// <param name="id">номер ячейки</param>
        /// <param name="var">Байтовое значение</param>
        public void SetInt32Value(string tabel, string column, int id, Int32 var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            while (db.Tabels[t].count < id) GrowTable(t);
                            byte[] arr = BitConverter.GetBytes(var);
                            db.Tabels[t].Columns[c].value[id * 2] = arr[0];
                            db.Tabels[t].Columns[c].value[id * 2 + 1] = arr[1];
                            db.Tabels[t].Columns[c].value[id * 2 + 2] = arr[2];
                            db.Tabels[t].Columns[c].value[id * 2 + 3] = arr[3];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// /// <param name="id">номер ячейки</param>
        /// <param name="var">Байтовое значение</param>
        public void SetInt64Value(string tabel, string column, int id, Int64 var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            while (db.Tabels[t].count < id) GrowTable(t);
                            byte[] arr = BitConverter.GetBytes(var);
                            db.Tabels[t].Columns[c].value[id * 2] = arr[0];
                            db.Tabels[t].Columns[c].value[id * 2 + 1] = arr[1];
                            db.Tabels[t].Columns[c].value[id * 2 + 2] = arr[3];
                            db.Tabels[t].Columns[c].value[id * 2 + 3] = arr[4];
                            db.Tabels[t].Columns[c].value[id * 2 + 4] = arr[5];
                            db.Tabels[t].Columns[c].value[id * 2 + 5] = arr[6];
                            db.Tabels[t].Columns[c].value[id * 2 + 6] = arr[7];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Добавляет новую строку в таблицу
        /// </summary>
        /// <param name="tabel">таблица</param>
        /// <param name="column">колонка</param>
        /// /// <param name="id">номер ячейки</param>
        /// <param name="var">Байтовое значение</param>
        public void SetStrValue(string tabel, string column, int id, string var)
        {
            for (int t = 0; t < db.Tabels.Length; t++)
            {
                if (db.Tabels[t].Name == tabel)
                {
                    for (int c = 0; c < db.Tabels[t].Columns.Length; c++)
                    {
                        if (db.Tabels[t].Columns[c].Name == column)
                        {
                            while (db.Tabels[t].count < id) GrowTable(t);
                            byte[] arr = Eshiba.DataConverter.StrToByte(var, 128);
                            for (int i = 0; i < 256; i++)
                            {
                                db.Tabels[t].Columns[c].value[id*256 + i] = arr[i];
                            }
                        }
                    }
                }
            }
        }


    }

}





