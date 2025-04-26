using Devices;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.FileControllers
{
    public class DBFileController : FileController
    {
        private readonly string connectionString;

        public DBFileController(string connectionString)
        {
            this.connectionString = connectionString;
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {
                connection.Open();
                int indexInFileContent = 0;
                using(SqlCommand command = new SqlCommand("SELECT Count(*) From Device", connection))
                    fileContent = new string[(int)command.ExecuteScalar()];
                Console.WriteLine(fileContent.Length);
                using (SqlCommand command = new SqlCommand("SELECT * FROM PersonalComputer INNER JOIN Device D on D.Id = PersonalComputer.DeviceId", connection))
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if (dataReader.IsDBNull(1))
                            fileContent[indexInFileContent++] = dataReader.GetString(2) + "," + dataReader.GetString(4) + "," + dataReader.GetValue(5).ToString();
                        else
                            fileContent[indexInFileContent++] = dataReader.GetString(2) + "," + dataReader.GetString(4) + "," + dataReader.GetValue(5).ToString() + "," + dataReader.GetString(1);
                    }
                    dataReader.Close();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM Smartwatch INNER JOIN Device D on Smartwatch.DeviceId = D.Id", connection))
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        fileContent[indexInFileContent++] = dataReader.GetString(2) + "," + dataReader.GetString(4) + "," + dataReader.GetValue(5).ToString() + "," + dataReader.GetValue(1).ToString() + "%";
                    }
                    dataReader.Close();
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM EmbeddedDevice INNER JOIN Device D on EmbeddedDevice.DeviceId = D.Id", connection))
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        fileContent[indexInFileContent++] = dataReader.GetString(3) + "," + dataReader.GetString(5) + "," + dataReader.GetValue(6).ToString() + "," + dataReader.GetString(1) + "," + dataReader.GetString(2);
                    dataReader.Close();
                }
            }
        }

        public override int FileLinesCount() => fileContent.Length;

        public override bool GetFileLine(int line, out string content)
        {
            try
            {
                content = fileContent[line];
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                content = null;
                return false;
            }
        }

        public override void SaveToFile(string newText) { }
    }
}
