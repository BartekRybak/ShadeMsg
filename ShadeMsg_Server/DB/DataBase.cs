using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;

namespace ShadeMsg_Server.DB
{
    public partial class DataBase
    {
        private static readonly string db_path = "DataBase/Users.db";

        protected static SQLiteConnection GetConnection(string databasePath)
        {
            SQLiteConnection sql = new SQLiteConnection("data source=" + databasePath);
            sql.Open();
            return sql;
        }

        public static string ArrayToField(string[] data,char separator)
        {
            string field = string.Empty;

            for(int i =0;i< data.Length;i++)
            {
                
                if (i < data.Length - 1)
                {
                    field += data[i] + separator.ToString();
                }
                else
                {
                    field += data[i];
                }
            }
            return field;
        }

        public static string[] FieldToArray(string data, char separator)
        {
            return data.Split(separator);
        }

        public static string[] AddItemToArray(string[] array,string item)
        {
            List<string> list = new List<string>();
            foreach(string i in array)
            {
                if(i != string.Empty)
                {
                    list.Add(i);
                }
            }
            list.Add(item);
            return list.ToArray();
        }

        public static string[] DellItemFromArray(string[] array,string item)
        {
            List<string> list = new List<string>();
            foreach(string i in array)
            {
                if(i != item && i != string.Empty)
                {
                    list.Add(i);
                }
            }
            return list.ToArray();
        }
    }
}
