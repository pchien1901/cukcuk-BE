//using core.Entities;
using Dapper;
using MISA.CUKCUK.Core.DTOs.CrudDTOs;
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

        public List<EmployeeInfo> GetEmployeeInfo()
        {
            // Truy vấn
            //var sql = $"SELECT e.*, d.DepartmentName, p.PositionName  FROM Employee e" +
            //    $"  INNER JOIN Department d ON e.DepartmentId = d.DepartmentId" +
            //    $"  LEFT JOIN `Position` p ON e.PositionId = p.PositionId;";

            var procGetEmployeeInfo = "Proc_GetEmployeeInfo";

            // Thực hiện truy vấn
            var data = _dbContext.Connection.Query<EmployeeInfo>(procGetEmployeeInfo, commandType: System.Data.CommandType.StoredProcedure).ToList();

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

        public int GetPageCount<EmployeeInfo>(int pageSize, string? text)
        {
            // tên procedure
            var proc = "Proc_GetEmployeeInfoPageCount";
            var searchText = "";
            if (!String.IsNullOrEmpty(text))
            {
                searchText = text;
            }
            var param = new DynamicParameters();
            param.Add("text", searchText);

            // thực hiện truy vấn
            var totalRecords = _dbContext.Connection.QuerySingle<int>(proc, param, commandType: System.Data.CommandType.StoredProcedure);

            return (int)Math.Ceiling((double)totalRecords / pageSize);
        }

        public Page<EmployeeInfo> GetEmployeeInfoByPage(int offset, int pageSize, string? text)
        {
            /// Lấy danh sách kết quả phân trang - tìm kiếm
            // tên procedure
            var proc = "Proc_GetEmployeeInfoByPage";

            var param = new DynamicParameters();
            param.Add("offset", offset);
            param.Add("pageSize", pageSize);
            param.Add("text", text);

            var listRecord = _dbContext.Connection.Query<EmployeeInfo>(proc, param, commandType: System.Data.CommandType.StoredProcedure).ToList();

            /// Lấy tổng số trang
            // tên procedure
            var procPageCount = "Proc_GetEmployeeInfoPageCount";
            var searchText = "";
            if (!String.IsNullOrEmpty(text))
            {
                searchText = text;
            }
            var paramPageCount = new DynamicParameters();
            paramPageCount.Add("text", searchText);

            // thực hiện truy vấn
            var totalRecords = _dbContext.Connection.QuerySingle<int>(procPageCount , param: paramPageCount, commandType: System.Data.CommandType.StoredProcedure);

            var pageCount = (int)Math.Ceiling((double)totalRecords / pageSize);

            /// Tổng sống bản ghi
             // tên procedure
            var procTotalRecord = "Proc_CountEmployeeSearchResult";
            var searchTotalRecordText = "";
            if (!String.IsNullOrEmpty(text))
            {
                searchTotalRecordText = text;
            }
            var paramTotalRecord = new DynamicParameters();
            paramTotalRecord.Add("text", searchTotalRecordText);

            // thực hiện truy vấn
            var totalRecordsToCount = _dbContext.Connection.QuerySingle<int>(procTotalRecord, param: paramTotalRecord, commandType: System.Data.CommandType.StoredProcedure);
            return new Page<EmployeeInfo>
            {
                ListRecord = listRecord,
                CurrentPage = offset,
                TotalPage = pageCount,
                TotalRecord = totalRecordsToCount
            };
        }

        public int CountSearchRecord(string? text)
        {
            // tên procedure
            var proc = "Proc_CountEmployeeSearchResult";
            var searchText = "";
            if (!String.IsNullOrEmpty(text))
            {
                searchText = text;
            }
            var param = new DynamicParameters();
            param.Add("text", searchText);

            // thực hiện truy vấn
            var totalRecords = _dbContext.Connection.QuerySingle<int>(proc, param, commandType: System.Data.CommandType.StoredProcedure);
            return totalRecords;
        }
        #endregion
    }
}
