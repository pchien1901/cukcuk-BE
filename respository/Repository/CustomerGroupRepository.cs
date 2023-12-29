using core.Entities;
using Dapper;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class CustomerGroupRepository : MISADatabaseContext, IDisposable, ICustomerGroupRepository
    {
        /// <summary>
        /// Đóng connection khi không sử dụng
        /// </summary>
        /// Created by: PMCHIEN
        public void Dispose()
        {
            connection.Dispose();
        }

        /// <summary>
        /// Lấy danh sách CustomerGroup
        /// </summary>
        /// <returns>List customerGroup</returns>
        /// Created by: PMCHIEN
        public List<CustomerGroup> Get()
        {
            // câu truy vấn
            var sql = "SELECT * FROM CustomerGroup";

            // Lấy dữ liệu
            var data = connection.Query<CustomerGroup>(sql).ToList();

            // trả về kết quả
            return data;
        }

        /// <summary>
        /// Lấy CustomerGroup theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1 bản ghi có Id trùng lặp</returns>
        /// Created by: PMCHIEN
        public CustomerGroup Get(string id)
        {
            // Truy vấn
            var sql = $"SELECT * FROM CustomerGroup WHERE CustomerGroupId = @id";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", id);

            // Thực hiện truy vấn
            var data = connection.QueryFirstOrDefault<CustomerGroup>(sql, param: parameters);

            // Trả về kết quả
            return data;
        }

        /// <summary>
        /// Lấy các bản ghi có cùng CustomerGroupName
        /// </summary>
        /// <param name="customerGroupName"></param>
        /// <returns></returns>
        /// Created by: PMCHIEN
        public List<CustomerGroup> GetByName(string customerGroupName)
        {
            // Truy vấn
            var sql = $"SELECT * FROM CustomerGroup WHERE CustomerGroupName = @name";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@name", customerGroupName);

            // Thực hiện truy vấn
            var data = connection.Query<CustomerGroup>(sql, param: parameters).ToList();

            // Trả về kết quả
            return data;
        }

        /// <summary>
        /// Thêm một bản ghi vào 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Số bản ghi bị thay đổi</returns>
        /// Created by: PMCHIEN
        public int Insert(CustomerGroup customerGroup)
        {
            // thêm id cho customerGroup
            customerGroup.CustomerGroupId = Guid.NewGuid();

            //
            var colNameList = "";
            var colParamList = "";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();

            // lấy properties của Customer
            var props = typeof(CustomerGroup).GetProperties();

            // duyệt từng prop
            foreach (var prop in props)
            {
                // lấy tên prop
                var propName = prop.Name;

                if (
                    propName != "CreatedDate" && propName != "ModifiedDate"
                    )
                {
                    // lấy value
                    var propValue = prop.GetValue(customerGroup);
                    colNameList = colNameList + $"{propName},";
                    colParamList = colParamList + $"@{propName},";
                    parameters.Add($"@{propName}", propValue);
                }

            }

            colNameList = colNameList.Substring(0, colNameList.Length - 1);
            colParamList = colParamList.Substring(0, colParamList.Length - 1);

            if (customerGroup.CreatedDate is null || customerGroup.CreatedDate == "")
            {
                colNameList = colNameList + ", CreatedDate";
                colParamList = colParamList + $", @CreatedDate";
                parameters.Add("@CreatedDate", DateTime.Now);
            }
            else
            {
                colNameList = colNameList + ", CreatedDate";
                colParamList = colParamList + $", @CreatedDate";
                if(DateTime.TryParse(customerGroup.CreatedDate, out DateTime createdDate))
                {
                    parameters.Add("@CreatedDate", createdDate);
                }
            }

            if (customerGroup.ModifiedDate != null && customerGroup.ModifiedDate != "")
            {
                colNameList = colNameList + ", ModifiedDate";
                colParamList = colParamList + $", @ModifiedDate";
                if (DateTime.TryParse(customerGroup.ModifiedDate, out DateTime modifiedDate))
                {
                    parameters.Add("@ModifiedDate", modifiedDate);
                }
                else
                {
                    parameters.Add("@ModifiedDate", null);
                }

            }

            // Truy vấn
            var sql = $"INSERT INTO CustomerGroup ({colNameList}) VALUES ({colParamList})";

            // Thực hiện truy vấn
            var res = connection.Execute(sql, param: parameters);

            // Trả về kết quả
            return res;
        }

        /// <summary>
        /// Thêm một bản ghi
        /// </summary>
        /// <param name="customerGroup"></param>
        /// <returns></returns>
        /// Created by: PMCHIEN
        public int Update(CustomerGroup customerGroup)
        {
            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();

            // lấy properties của Customer
            var props = typeof(CustomerGroup).GetProperties();

            // tạo danh sách cột và giá trị cập nhật
            var updateList = "";

            // duyệt từng prop
            foreach (var prop in props)
            {
                // lấy tên prop
                var propName = prop.Name;
                // lấy value
                var propValue = prop.GetValue(customerGroup);

                // bỏ qua trường CustomerGroupId
                if (propName != "CustomerGroupId" && propName != "CreatedDate" &&
                    propName != "ModifiedDate")
                {
                    updateList = updateList + $"{propName} = @{propName},";
                    parameters.Add($"@{propName}", propValue);
                }

            }

            if (customerGroup.CreatedDate != null || customerGroup.CreatedDate != "")
            {
                updateList = updateList + "CreatedDate = @CreatedDate,";
                var createdDate = Convert.ToDateTime(customerGroup.CreatedDate);
                /*
            if (DateTime.TryParse(customerGroup.CreatedDate, out DateTime createdDate))
            {
                parameters.Add("@CreatedDate", createdDate);
            }
            else
            {
                parameters.Add("@CreatedDate", null);
            }
                */
                parameters.Add("@CreatedDate", createdDate);
            }
            updateList = updateList + "ModifiedDate = @ModifiedDate,";
            parameters.Add("@ModifiedDate", DateTime.Now);

            updateList = updateList.Substring(0, updateList.Length - 1);

            // Truy vấn
            var sql = $"UPDATE CustomerGroup " +
                $"SET {updateList} " +
                $"WHERE CustomerGroupId = @CustomerGroupId";

            parameters.Add("@CustomerGroupId", customerGroup.CustomerGroupId);

            // Thực hiện truy vấn
            var res = connection.Execute(sql, param: parameters);

            // Trả về kết quả
            return res;
        }

        public int Delete(string customerGroupId)
        {
            // Truy vấn
            var sql = $"DELETE FROM CustomerGroup WHERE CustomerGroupId = @id";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", customerGroupId);

            // Thực hiện truy vấn
            var data = connection.Execute(sql, param: parameters);

            // Trả về kết quả
            return data;
        }

        public int DeleteAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Kiểm tra CustomerGroupName
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true - CustomerGroupName đã tồn tại, false - chưa tồn tại</returns>
        public bool CheckNameIsExist(string name)
        {
            // truy vấn
            string sql = $"SELECT * FROM CustomerGroup WHERE CustomerGroupName = @customerGroupName";

            // DynamicParameters chống sql injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerGroupName", name);

            // thực hiện truy vấn
            var data = connection.QueryFirstOrDefault<CustomerGroup>(sql, param: parameters);

            // nếu không có kết quả => Name chưa tồn tại, trả về false
            if (data == null)
            {
                return false;
            }

            return true;
        }
    }
}
