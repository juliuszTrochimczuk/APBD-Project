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
    public class DBFileController : FileController, IDatabaseHandler
    {
        private readonly string connectionString;

        public DBFileController(string connectionString)
        {
            this.connectionString = connectionString;
            using (SqlConnection connection = new(connectionString)) 
            {
                connection.Open();
                int indexInFileContent = 0;
                using(SqlCommand command = new("SELECT Count(*) From Device", connection))
                    fileContent = new string[(int)command.ExecuteScalar()];
                using (SqlCommand command = new("SELECT * FROM PersonalComputer INNER JOIN Device D on D.Id = PersonalComputer.DeviceId", connection))
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
                using (SqlCommand command = new("SELECT * FROM Smartwatch INNER JOIN Device D on Smartwatch.DeviceId = D.Id", connection))
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        fileContent[indexInFileContent++] = dataReader.GetString(2) + "," + dataReader.GetString(4) + "," + dataReader.GetValue(5).ToString() + "," + dataReader.GetValue(1).ToString() + "%";
                    }
                    dataReader.Close();
                }
                using (SqlCommand command = new("SELECT * FROM EmbeddedDevice INNER JOIN Device D on EmbeddedDevice.DeviceId = D.Id", connection))
                {
                    SqlDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        fileContent[indexInFileContent++] = dataReader.GetString(3) + "," + dataReader.GetString(5) + "," + dataReader.GetValue(6).ToString() + "," + dataReader.GetString(1) + "," + dataReader.GetString(2);
                    dataReader.Close();
                }
            }
        }

        public void AddDevice(Device deviceToAdd)
        {
            if ((deviceToAdd is not Smartwatch) && (deviceToAdd is not PersonalComputer) && (deviceToAdd is not EmbeddedDevice))
                return;

            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new($"INSERT INTO Device(Id, Name, IsTurnedOn) VALUES({deviceToAdd.Id}, {deviceToAdd.Name}, {deviceToAdd.IsTurnedOn})", connection))
                {
                    int result = command.ExecuteNonQuery();
                    if (result <= 0)
                        Console.WriteLine("NOT AFFECTED");
                }
                if (deviceToAdd is Smartwatch sw)
                {
                    using (SqlCommand command = new($"INSERT INTO Smartwatch(BatteryPercentage, DeviceId) VALUES({sw.BatteryLevel}, {deviceToAdd.Id})", connection))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                            Console.WriteLine("NOT AFFECTED");
                    }
                }
                else if (deviceToAdd is PersonalComputer pc)
                {
                    using (SqlCommand command = new($"INSERT INTO PersonalComputer(OperationSystem, DeviceId) VALUES({pc.OperatingSystem}, {deviceToAdd.Id})", connection))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                            Console.WriteLine("NOT AFFECTED");
                    }
                }
                else if (deviceToAdd is EmbeddedDevice ed)
                {
                    using (SqlCommand command = new($"INSERT INTO EmbeddedDevice(IpAddress, NetworkName, DeviceId) VALUES({ed.IpAdress}, {ed.NetworkName}, {deviceToAdd.Id})", connection))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                            Console.WriteLine("NOT AFFECTED");
                    }
                }
            }
            throw new NotImplementedException();
        }

        public void DeleteDevice(Device deviceToDelete)
        {
            if ((deviceToDelete is not Smartwatch) && (deviceToDelete is not PersonalComputer) && (deviceToDelete is not EmbeddedDevice))
                return;

            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();
                if (deviceToDelete is Smartwatch)
                {
                    using (SqlCommand command = new($"DELETE FROM Smartwatch WHERE DeviceId = {deviceToDelete.Id}", connection))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                            Console.WriteLine("NOT AFFECTED");
                    }
                }
                else if (deviceToDelete is PersonalComputer)
                {
                    using (SqlCommand command = new($"DELETE FROM PersonalComputer WHERE DeviceId = {deviceToDelete.Id}", connection))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                            Console.WriteLine("NOT AFFECTED");
                    }
                }
                else if (deviceToDelete is EmbeddedDevice)
                {
                    using (SqlCommand command = new($"DELETE FROM EmbeddedDevice WHERE DeviceId = {deviceToDelete.Id}", connection))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                            Console.WriteLine("NOT AFFECTED");
                    }
                }

                using (SqlCommand command = new($"DELETE FROM Device WHERE Id = {deviceToDelete.Id}", connection))
                {
                    int result = command.ExecuteNonQuery();
                    if (result <= 0)
                        Console.WriteLine("NOT AFFECTED");
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

        public void UpdateDevice(Device deviceToUpdate)
        {
            if ((deviceToUpdate is not Smartwatch) && (deviceToUpdate is not PersonalComputer) && (deviceToUpdate is not EmbeddedDevice))
                return;

            using (SqlConnection connection = new())
            {
                if (deviceToUpdate is Smartwatch sw)
                {
                    using (SqlCommand command = new($"UPDATE Smartwatch SET BatteryPercentage = {sw.BatteryLevel} WHERE DeviceId = {deviceToUpdate.Id}", connection))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                            Console.WriteLine("NOT AFFECTED");
                    }
                }
                else if (deviceToUpdate is PersonalComputer pc)
                {
                    using (SqlCommand command = new($"UPDATE PersonalComputer SET OperationSystem = {pc.OperatingSystem} WHERE DeviceId = {deviceToUpdate.Id}", connection))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                            Console.WriteLine("NOT AFFECTED");
                    }
                }
                else if (deviceToUpdate is EmbeddedDevice ed)
                {
                    using (SqlCommand command = new($"UPDATE EmbeddedDevice SET IpAddress = {ed.IpAdress}, NetworkName = {ed.NetworkName} WHERE DeviceId = {deviceToUpdate.Id}", connection))
                    {
                        int result = command.ExecuteNonQuery();
                        if (result <= 0)
                            Console.WriteLine("NOT AFFECTED");
                    }
                }

                connection.Open();
                using (SqlCommand command = new($"UPDATE Device SET Name = {deviceToUpdate.Name}, IsTurnedOn = {deviceToUpdate.IsTurnedOn} WHERE Id = {deviceToUpdate.Id}", connection))
                {
                    int result = command.ExecuteNonQuery();
                    if (result <= 0)
                        Console.WriteLine("NOT AFFECTED");
                }
            }
        }
    }
}
