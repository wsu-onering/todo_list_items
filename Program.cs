using System;
using System.Linq;
using System.Globalization;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace todo_list_items
{
    class Program
    {
        enum Actions {List, Reset};
        static void PrintInfo(string connectionString, string table){
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string cmd = "SELECT * FROM {0}";
            cmd = String.Format(cmd, table);

            SqlCommand command = new SqlCommand(cmd, sqlConnection);
            List<List<string>> rows = new List<List<string>>();
            using (SqlDataReader reader = command.ExecuteReader()) {
                while (reader.Read()) {
                    // On the first go around, add a row the names of each column
                    if (rows.Count == 0) {
                        List<string> nameRow = new List<string>();
                        for (int i  = 0; i < reader.FieldCount; i++) {
                            nameRow.Add(String.Format("{0}", reader.GetName(i)));
                        }
                        rows.Add(nameRow);
                    }
                    List<string> row = new List<string>();
                    for (int i  = 0; i < reader.FieldCount; i++) {
                        row.Add(String.Format("{0}", reader[i]).Trim());
                    }
                    rows.Add(row);
                }
            }
            if (rows.Count > 0) {
                // Here we need to find the maximum width of each column so
                // we may print the data with the correct padding.
                int[] colWidths = new int[rows[0].Count];
                foreach (List<string> row in rows) {
                    // Sadly this Select is the only way to get
                    // functionality like python's 'Enumerate' functionality
                    // for collections.
                    foreach (var it in row.Select((x,i) => new {Value = x, Index = i})) {
                        StringInfo info = new StringInfo(it.Value);
                        if (colWidths[it.Index] < info.LengthInTextElements) {
                            colWidths[it.Index] = info.LengthInTextElements;
                        }
                    }
                }
                // Now print each row with the correct ammount of padding
                // for each column
                foreach (List<string> row in rows) {
                    string line = "";
                    foreach (var it in row.Select((x,i) => new {Value = x, Index = i})) {
                        string formatStr = "{0," + String.Format("{0}", colWidths[it.Index]) + "} ";
                        line += String.Format(formatStr, it.Value);
                    }
                    Console.WriteLine(line);
                }
            }
        }
        static void Reset(string connectionString, string table) {
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string cmd = "UPDATE {0} SET CompleteDate=NULL, IsComplete='False'";
            cmd = String.Format(cmd, table);

            Console.WriteLine("Resetting the all todo items in the database.");
            SqlCommand command = new SqlCommand(cmd, sqlConnection);
            command.ExecuteNonQuery();
        }
        static void Main(string[] args)
        {
            Actions action = Actions.List;
            Console.WriteLine(String.Format("Number of arguments: {0}", args.Length));
            if (args.Length > 0) {
                switch (args[0]) {
                    case "list":
                        action = Actions.List;
                        break;
                    case "reset":
                        action = Actions.Reset;
                        break;
                    default:
                        action = Actions.List;
                        break;
                }
            }

            string connectionString = "Server=tcp:todositeserver.database.windows.net,1433;Initial Catalog=TodoUserDatabase;Persist Security Info=False;User ID=TodoAdmin;Password=TodoPass9;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            //SqlConnection sqlConnection = new SqlConnection(connectionString);
            //sqlConnection.Open();

            // Print our list of rows from the DB
            if (action == Actions.List) {
                PrintInfo(connectionString, "TodoTable");
                PrintInfo(connectionString, "TodoTable2");
            // Reset all ToDo items so that they're not complete and their
            // "CompleteDate" field is NULL.
            } else if (action == Actions.Reset) {
                Reset(connectionString, "TodoTable");
                Reset(connectionString, "TodoTable2");
            }
            //sqlConnection.Close();

            Console.WriteLine("Hello World!");
        }
    }
}
