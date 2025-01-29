using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace OptikProjeTumSayfa
{
    public class DbConnection
    {
        private readonly string connectionString;

        public DbConnection()
        {
            // App.config dosyasından bağlantı dizesini al
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<(int, string, string, string, string, string, string, string, string)> GetStudents()
        {
            List<(int, string, string, string, string, string, string, string, string)> students = new List<(int, string, string, string, string, string, string, string, string)>();

            using (MySqlConnection connection = GetConnection())
            {
                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    string query = @"
                SELECT 
                    s.StudentID, 
                    s.Name, 
                    s.StudentNumber, 
                    s.TCKN, 
                    d.DepartmentName, 
                    IFNULL(e.ExamName, 'Belirtilmemiş') AS ExamName, 
                    IFNULL(e.ExamDate, '0000-00-00') AS ExamDate,
                    IFNULL(et.RoomNumber, 0) AS RoomNumber, 
                    IFNULL(et.SeatNumber, 0) AS SeatNumber
                FROM students s
                LEFT JOIN departments d ON s.DepartmentID = d.DepartmentID
                LEFT JOIN examdetails e ON s.ExampleDetailsID = e.ID
                LEFT JOIN examtitles et ON s.ExampleTitleID = et.ID";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var studentData = (
                                    reader.GetInt32("StudentID"),
                                    reader.GetString("Name"),
                                    reader["StudentNumber"].ToString(),
                                    reader["TCKN"].ToString(),
                                    reader.GetString("DepartmentName"),
                                    reader.GetString("ExamName"),
                                    reader["ExamDate"].ToString(),
                                    reader["RoomNumber"].ToString(),
                                    reader["SeatNumber"].ToString()
                                );

                                students.Add(studentData);
                                Console.WriteLine($"Öğrenci Yüklendi: {studentData}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veritabanı Hatası: " + ex.Message);
                }
            }
            return students;
        }






        // Bağlantı nesnesi döndüren bir metot
        public MySqlConnection GetConnection()
        {
            var connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Veritabanı bağlantı hatası: " + ex.Message);
                throw;
            }
            return connection;
        }
    }
}
