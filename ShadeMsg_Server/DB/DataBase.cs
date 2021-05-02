﻿using System;
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

        protected static string ArrayToField(string[] data,char separator)
        {
            string field = string.Empty;

            if(data.Length == 1)
            {
                field += data[0];
            }
            else
            {
                for (int i = 0; i < data.Length; i++)
                {
                    field += data[i];
                    if (i < data.Length - 1) { field += separator; }
                }
            }
            return field;
        }

        protected static string[] FieldToArray(string data, char separator)
        {
            return data.Split(separator);
        }
    }
}