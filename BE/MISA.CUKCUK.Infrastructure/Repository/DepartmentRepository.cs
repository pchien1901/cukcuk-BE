//using core.Entities;
using Dapper;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        #region Constructor
        public DepartmentRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }
        #endregion

        #region Method
        public bool CheckCodeIsExist(string code)
        {
            // truy vấn
            string sql = $"SELECT * FROM Department WHERE DepartmentCode = @code";

            // DynamicParameters chống sql injection
            var parameters = new DynamicParameters();
            parameters.Add("@code", code);

            // thực hiện truy vấn
            var data = _dbContext.Connection.QueryFirstOrDefault<Department>(sql, param: parameters);

            // nếu không có kết quả => CustomerCode chưa tồn tại, trả về false
            if (data == null)
            {
                return false;
            }

            return true;
        }

        public List<Department> GetByCode(string code)
        {
            // Truy vấn
            var sql = $"SELECT * FROM Department WHERE DepartmentCode = @code";

            // Dùng DynamicParameters chống SQL injection
            var parameters = new DynamicParameters();
            parameters.Add("@ode", code);

            // Thực hiện truy vấn
            var data = _dbContext.Connection.Query<Department>(sql, param: parameters).ToList();

            // Trả về kết quả
            return data;
        }

        #endregion
    }
}
