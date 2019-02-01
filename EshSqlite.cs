using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using System.Data.Common;
using System.IO;
using System.Data;
using Eshiba;



namespace Eshiba
{
    public class SQliteDB
    {
        string path = "";

        IniFile ini;

        public char scobka = '"';


        public SQliteDB(string conf)
        {
            ini = new IniFile(conf);
            path = ini.Read("database", "dbfile");
        }

        public string Read(string db, string column, string sql)
        {
            string data = null;

            SQLiteConnection connection =
            new SQLiteConnection(string.Format("Data Source={0};", path));

            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * from " + db + " " + sql + ";", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)

                Console.WriteLine(record[column]);

            connection.Close();

            return data;
        }

        /// <summary>
        /// Использовать для выборки значений по string параметру
        /// </summary>
        /// <param name="db"></param>
        /// <param name="column"></param>
        /// <param name="param"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Read(string db, string column, string param, string value)
        {
            string data = null;

            SQLiteConnection connection =
            new SQLiteConnection(string.Format("Data Source={0};", path));



            connection.Open();

            SQLiteCommand command = new SQLiteCommand("SELECT * from " + db + " where " + param + "=" + scobka + value + scobka + ";", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)

                data = record[column].ToString();



            connection.Close();

            return data;
        }

        public string Read(string db, string column)
        {
            string data = null;

            SQLiteConnection connection =
            new SQLiteConnection(string.Format("Data Source={0};", path));

            connection.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT * from " + db + ";", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)

                Console.WriteLine(record[column]);

            connection.Close();

            return data;
        }

        /// <summary>
        /// Использовать для выборки значений по int параметру
        /// </summary>
        /// <param name="db"></param>
        /// <param name="column"></param>
        /// <param name="param"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Read(string db, string column, string param, int value)
        {
            string data = null;

            SQLiteConnection connection =
            new SQLiteConnection(string.Format("Data Source={0};", path));



            connection.Open();

            SQLiteCommand command = new SQLiteCommand("SELECT * from " + db + " where " + param + "=" + scobka + value + scobka + ";", connection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)

                data = record[column].ToString();



            connection.Close();

            return data;
        }

        /// <summary>
        /// INSERT INTO 'example' ('id', 'value') VALUES (1, 'Вася');
        /// </summary>
        /// <param name="sql"></param>
        public void sql(string sql)
        {
            SQLiteConnection connection =
                 new SQLiteConnection(string.Format("Data Source={0};", path));

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, connection);

            command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// бд,колонка,значение,по колонке,со значением
        /// </summary>
        /// <param name="db"></param>
        /// <param name="column"></param>
        /// <param name="var"></param>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <param name="pvalue"></param>
        public void write(string db, string column, string var, string param, string value)
        {
            string sql = "UPDATE " + db + " SET " + column + "='" + var + "' WHERE " + param + "='" + value + "';";

            SQLiteConnection connection =
 new SQLiteConnection(string.Format("Data Source={0};", path));

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, connection);

            command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// бд,колонка,значение,по колонке,со значением
        /// </summary>
        /// <param name="db"></param>
        /// <param name="column"></param>
        /// <param name="var"></param>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <param name="pvalue"></param>
        public void write(string db, string column, int var, string param, string value)
        {
            string sql = "UPDATE " + db + " SET " + column + "=" + var + " WHERE " + param + "='" + value + "';";

            SQLiteConnection connection =
            new SQLiteConnection(string.Format("Data Source={0};", path));

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, connection);

            command.ExecuteNonQuery();
            connection.Close();
        }

        /// <summary>
        /// бд,колонка,значение,по колонке,со значением
        /// </summary>
        /// <param name="db"></param>
        /// <param name="column"></param>
        /// <param name="var"></param>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <param name="pvalue"></param>
        public void write(string db, string column, string var, string param, int value)
        {
            string sql = "UPDATE " + db + " SET " + column + "=" + var + " WHERE " + param + "='" + value + "';";

            SQLiteConnection connection =
 new SQLiteConnection(string.Format("Data Source={0};", path));

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, connection);

            command.ExecuteNonQuery();
            connection.Close();
        }
        /// <summary>
        /// бд,колонка,значение,по колонке,со значением
        /// </summary>
        /// <param name="db"></param>
        /// <param name="column"></param>
        /// <param name="var"></param>
        /// <param name="value"></param>
        /// <param name="param"></param>
        /// <param name="pvalue"></param>
        public void write(string db, string column, int var, string param, int value)
        {
            string sql = "UPDATE " + db + " SET " + column + "=" + var + " WHERE " + param + "=" + value + ";";

            SQLiteConnection connection =
 new SQLiteConnection(string.Format("Data Source={0};", path));

            connection.Open();

            SQLiteCommand command = new SQLiteCommand(sql, connection);

            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}

//"INSERT INTO 'example' ('id', 'value') VALUES (1, 'Вася');"
