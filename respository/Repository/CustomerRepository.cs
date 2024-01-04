using core.Entities;
using Dapper;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class CustomerRepository : MISABaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(IMISADbContext dbContext) : base(dbContext)
        {
        }


        /// <summary>
        /// Lấy bản ghi theo customerCode
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns>List<Customer></returns>
        /// Created By: PMCHIEN(27/12/2023)
        public List<Customer> GetByCode(string customerCode)
        {
            // Truy vấn
            var sql = $"SELECT * FROM Customer WHERE CustomerCode = @customerCode";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerCode", customerCode);

            // Thực hiện truy vấn
            var data = _dbContext.Connection.Query<Customer>(sql, param: parameters).ToList();

            // Trả về kết quả
            return data;
        }

        /// <summary>
        /// Kiểm tra CustomerCode đã tồn tại chưa
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns>
        /// true - đã tồn tại, 
        /// false - chưa tồn tại
        /// </returns>
        /// Created By: PMCHIEN(27/12/2023)
        public bool CheckCodeIsExist(string customerCode)
        {
            // truy vấn
            string sql = $"SELECT * FROM Customer WHERE CustomerCode = @customerCode";

            // DynamicParameters chống sql injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerCode", customerCode);

            // thực hiện truy vấn
            var data = _dbContext.Connection.QueryFirstOrDefault<Customer>(sql, param: parameters);

            // nếu không có kết quả => CustomerCode chưa tồn tại, trả về false
            if (data == null)
            {
                return false;
            }

            return true;
        }
    }
}
