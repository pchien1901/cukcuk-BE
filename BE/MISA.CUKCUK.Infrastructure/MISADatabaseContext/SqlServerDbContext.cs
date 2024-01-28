using MISA.CUKCUK.Core.DTOs.CrudDTOs;
using MISA.CUKCUK.Infrastructure.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.MISADatabaseContext
{
    public class SqlServerDbContext : IMISADbContext
    {
        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        public SqlServerDbContext()
        {
            Connection = new SqlConnection("");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Get<T>()
        {
            throw new NotImplementedException();
        }

        public T? Get<T>(string id)
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public int Update<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public int Delete<T>(string id)
        {
            throw new NotImplementedException();
        }

        public int DeleteAny<T>(string[] deleteAny)
        {
            throw new NotImplementedException();
        }
    }
}
