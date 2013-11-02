using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Sales_v_2_0
{
    static class DataBaseQuerys
    {
        public static void InsertQuery(string path, string query, 
            OleDbParameter[] parameters)
        {
            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; " +
            "Data Source=" + path);
            connection.Open();
            OleDbTransaction trns = connection.BeginTransaction();
            OleDbCommand command = new OleDbCommand(query, connection);
            command.Transaction = trns;
            if (parameters!=null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.Add(parameters[i]);
                }
            }
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.InsertCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
        }

        public static void UpdateQuery(string path, string query,
            OleDbParameter[] parameters)
        {
            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; " +
            "Data Source=" + path);
            connection.Open();
            OleDbCommand command = new OleDbCommand(query, connection);
            if (parameters != null)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    command.Parameters.Add(parameters[i]);
                }
            }
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.UpdateCommand = command;
            adapter.InsertCommand.ExecuteNonQuery();
        }

        public static OleDbDataReader SelectQuary(string path,
            string query, OleDbParameter[] parameters)
        {
            OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; " +
            "Data Source=" + path);
            connection.Open();
            OleDbCommand command = new OleDbCommand(query, connection);

            if (parameters != null)
            {
                for (int j = 0; j < parameters.Length; j++)
                {
                    command.Parameters.Add(parameters[j]);
                }
            }
            OleDbDataReader reader = command.ExecuteReader();

            return reader;
        }

        public static void ListViewFilling(OleDbDataReader reader, ListView list)
        {
            list.Items.Clear();
            while (reader.Read())
            {
                ListViewItem item = new ListViewItem(reader[0].ToString());
                for (int i = 1; i < reader.FieldCount; i++)
                {
                    item.SubItems.Add(reader[i].ToString());
                }
                list.Items.Add(item);
            }
            reader.Close();
        }
    }
}
