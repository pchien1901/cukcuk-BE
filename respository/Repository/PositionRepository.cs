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
    public class PositionRepository : BaseRepository<Position>, IPositionRepository
    {
        #region Constructor
        public PositionRepository(IMISADbContext dbContext) : base(dbContext)
        {

        }
        #endregion

        #region Method
        public bool CheckNameIsExist(string name)
        {
            // truy vấn
            string sql = $"SELECT * FROM Position WHERE PositionName = @name";

            // DynamicParameters chống sql injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@name", name);

            // thực hiện truy vấn
            var data = _dbContext.Connection.QueryFirstOrDefault<Position>(sql, param: parameters);

            // nếu không có kết quả => Name chưa tồn tại, trả về false
            if (data == null)
            {
                return false;
            }

            return true;
        }

        public List<Position> GetByName(string name)
        {
            // Truy vấn
            var sql = $"SELECT * FROM Position WHERE PositionName = @name";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@name", name);

            // Thực hiện truy vấn
            var data = _dbContext.Connection.Query<Position>(sql, param: parameters).ToList();

            // Trả về kết quả
            return data;
        }
        #endregion
    }
}
