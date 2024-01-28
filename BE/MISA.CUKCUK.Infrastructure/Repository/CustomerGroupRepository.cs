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
    public class CustomerGroupRepository : BaseRepository<CustomerGroup>, ICustomerGroupRepository
    {
        #region Constructor
        public CustomerGroupRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }
        #endregion

        #region Method
        /// <summary>
        /// Lấy các bản ghi có cùng CustomerGroupName
        /// </summary>
        /// <param name="customerGroupName">giá trị của CustomerGroupName</param>
        /// <returns>Danh sách bản ghi</returns>
        /// Created by: PMCHIEN (27/12/2023)
        public List<CustomerGroup> GetByName(string customerGroupName)
        {
            // Truy vấn
            var sql = $"SELECT * FROM CustomerGroup WHERE CustomerGroupName = @name";

            // Dùng DynamicParameters chống SQL injection
            var parameters = new DynamicParameters();
            parameters.Add("@name", customerGroupName);

            // Thực hiện truy vấn
            var data = _dbContext.Connection.Query<CustomerGroup>(sql, param: parameters).ToList();

            // Trả về kết quả
            return data;
        }

        /// <summary>
        /// Kiểm tra CustomerGroupName đã tồn tại chưa
        /// </summary>
        /// <param name="name">CustomerGroupName cần kiểm tra</param>
        /// <returns>true - CustomerGroupName đã tồn tại, false - chưa tồn tại</returns>
        /// Created by: PMCHIEN (27/12/2023)
        public bool CheckNameIsExist(string name)
        {
            // truy vấn
            string sql = $"SELECT * FROM CustomerGroup WHERE CustomerGroupName = @customerGroupName";

            // DynamicParameters chống sql injection
            var parameters = new DynamicParameters();
            parameters.Add("@customerGroupName", name);

            // thực hiện truy vấn
            var data = _dbContext.Connection.QueryFirstOrDefault<CustomerGroup>(sql, param: parameters);

            // nếu không có kết quả => Name chưa tồn tại, trả về false
            if (data == null)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
