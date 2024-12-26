using System;
using System.Configuration;
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
