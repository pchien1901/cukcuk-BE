using core.Entities;
using Dapper;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        #region Constructor
        public EmployeeRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }
        #endregion

        #region Method
        public bool CheckCodeIsExist(string code)
        {
            // truy vấn
            string sql = $"SELECT * FROM Employee WHERE EmployeeCode = @code";

            // DynamicParameters chống sql injection
            var parameters = new DynamicParameters();
            parameters.Add("@code", code);

            // thực hiện truy vấn
            var data = _dbContext.Connection.QueryFirstOrDefault<Employee>(sql, param: parameters 
                    , transaction: _dbContext.Transaction
                );

            // nếu không có kết quả => EmployeeCode chưa tồn tại, trả về false
            if (data == null)
            {
                return false;
            }

            return true;
        }

        public List<Employee> GetByCode(string employeeCode)
        {
            // Truy vấn
            var sql = $"SELECT * FROM Employee WHERE EmployeeCode = @code";

            // Dùng DynamicParameters chống SQL injection
            var parameters = new DynamicParameters();
            parameters.Add("@code", employeeCode);

            // Thực hiện truy vấn
            var data = _dbContext.Connection.Query<Employee>(sql, param: parameters).ToList();

            // Trả về kết quả
            return data;
        }

        public string GetMaxCode()
        {
            // Truy vấn
            var sql = $"SELECT MAX(EmployeeCode) FROM Employee";

            // Thực hiện truy vấn
            var data = _dbContext.Connection.ExecuteScalar<string>(sql);

            // Trả về kết quả
            return data;
        }
        #endregion
    }
}
