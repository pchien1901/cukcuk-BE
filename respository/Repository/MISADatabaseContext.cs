using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class MISADatabaseContext
    {
        // thông tin kết nối
        string _connectionString = "Host = 8.222.228.150;" +
            "Port = 3306;" +
            "Database = W08.PMCHIEN.MF1778;" +
            "User Id = nvmanh;" +
            "Password = 12345678;";
        

        protected IDbConnection connection;
        public MISADatabaseContext()
        {
            connection = new MySqlConnection(_connectionString);
        }
    }
}
