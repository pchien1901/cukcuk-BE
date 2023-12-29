using core.Entities;
using Dapper;
using MISA.CUKCUK.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.Repository
{
    public class CustomerRepository : MISADatabaseContext, ICustomerRepository, IDisposable
    {
        /// <summary>
        /// Xóa người dùng theo Id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>số bản ghi bị xóa</returns>
        /// Created By: PMCHIEN (27/12/2023)
        public int Delete(string id)
        {
            // Truy vấn
            var sql = $"DELETE FROM Customer WHERE CustomerId = @id";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", id);

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
        /// Hủy connection khi không sử dụng
        /// </summary>
        public void Dispose()
        {
            connection.Dispose();
        }

        /// <summary>
        /// Lấy tất cả người dùng
        /// </summary>
        /// <returns></returns>
        /// Created By : PMCHIEN
        public List<Customer> Get()
        {
            // câu truy vấn
            var sql = "SELECT * FROM Customer";

            // Lấy dữ liệu
            var data = connection.Query<Customer>(sql).ToList();

            // trả về kết quả
            return data;
        }

        /// <summary>
        /// Lấy một bản ghi theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// Created By: PMCHIEN
        public Customer Get(string id)
        {
            // Truy vấn
            var sql = $"SELECT * FROM Customer WHERE CustomerId = @id";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", id);

            // Thực hiện truy vấn
            var data = connection.QueryFirstOrDefault<Customer>(sql, param: parameters);

            // Trả về kết quả
            return data;
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
            var data = connection.Query<Customer>(sql, param: parameters).ToList();

            // Trả về kết quả
            return data;
        }

        /// <summary>
        /// Thêm bản ghi vào bảng
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Created By: PMCHIEN(27/12/2023)
        public int Insert(Customer customer)
        {
            // thêm id cho customer
            customer.CustomerId = Guid.NewGuid();

            // Nếu CreatedDate null thì thêm
            if (customer.CreatedDate is null || customer.CreatedDate.ToString() == "")
            {
                customer.CreatedDate = DateTime.Now.ToString();
            }

            // Nếu DateOfBirth = "" thì không set
            if (customer.DateOfBirth == "")
            {
                customer.DateOfBirth = null;
            }

            //
            var colNameList = "";
            var colParamList = "";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();

            // lấy properties của Customer
            var props = typeof(Customer).GetProperties();

            // duyệt từng prop
            foreach (var prop in props)
            {
                // lấy tên prop
                var propName = prop.Name;
                if (
                    propName != "DateOfBirth" &&
                    propName != "CreatedDate" &&
                    propName != "ModifiedDate"
                )
                {
                    // lấy value
                    var propValue = prop.GetValue(customer);

                    colNameList = colNameList + $"{propName},";
                    colParamList = colParamList + $"@{propName},";
                    parameters.Add($"@{propName}", propValue);
                }

            }

            colNameList = colNameList.Substring(0, colNameList.Length - 1);
            colParamList = colParamList.Substring(0, colParamList.Length - 1);

            // đổi kiểu của CreatedDate, ModifiedDate, DateOfBirth sang DateTime
            if (customer.CreatedDate is null || customer.CreatedDate.ToString() == "")
            {
                colNameList = colNameList + ", CreatedDate";
                colParamList = colParamList + $", @CreatedDate";
                parameters.Add("@CreatedDate", DateTime.Now);
            }
            if (customer.DateOfBirth != null && customer.DateOfBirth != "")
            {
                colNameList = colNameList + ", DateOfBirth";
                colParamList = colParamList + $", @DateOfBirth";
                if (DateTime.TryParse(customer.DateOfBirth, out DateTime dateOfBirth))
                {
                    parameters.Add("@DateOfBirth", dateOfBirth);
                }
                else
                {
                    parameters.Add("@DateOfBirth", null);
                }

            }
            if (customer.ModifiedDate != null && customer.ModifiedDate != "")
            {
                colNameList = colNameList + ", ModifiedDate";
                colParamList = colParamList + $", @ModifiedDate";
                if (DateTime.TryParse(customer.ModifiedDate, out DateTime modifiedDate))
                {
                    parameters.Add("@ModifiedDate", modifiedDate);
                }
                else
                {
                    parameters.Add("@ModifiedDate", null);
                }

            }

            // Truy vấn
            var sql = $"INSERT INTO Customer ({colNameList}) VALUES ({colParamList})";

            // Thực hiện truy vấn
            var res = connection.Execute(sql, param: parameters);

            // Trả về kết quả
            return res;
        }

        /// <summary>
        /// Thay đổi thông tin 
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Số bản ghi bị thay đổi</returns>
        /// Created By: PMCHIEN(27/12/2023)
        public int Update(Customer customer)
        {
            

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();

            // lấy properties của Customer
            var props = typeof(Customer).GetProperties();

            // tạo danh sách cột và giá trị cập nhật
            var updateList = "";

            // duyệt từng prop
            foreach (var prop in props)
            {
                // lấy tên prop
                var propName = prop.Name;
                if(
                    propName != "DateOfBirth" &&
                    propName != "CreatedDate" &&
                    propName != "ModifiedDate" &&
                    propName != "CustomerId"
                    )
                {
                    // lấy value
                    var propValue = prop.GetValue(customer);
                    updateList = updateList + $"{propName} = @{propName},";
                    parameters.Add($"@{propName}", propValue);
                }
            }

            // đổi kiểu của CreatedDate, ModifiedDate, DateOfBirth sang DateTime
            if (customer.CreatedDate != null || customer.CreatedDate != "")
            {
                updateList = updateList + "CreatedDate = @CreatedDate,";
                if(DateTime.TryParse(customer.CreatedDate, out DateTime createdDate))
                {
                    parameters.Add("@CreatedDate", createdDate);
                }
                else
                {
                    parameters.Add("@CreatedDate", null);
                }
            }
            if (customer.DateOfBirth != null && customer.DateOfBirth != "")
            {
                updateList = updateList + "DateOfBirth = @DateOfBirth,";
                if (DateTime.TryParse(customer.DateOfBirth, out DateTime dateOfBirth))
                {
                    parameters.Add("@DateOfBirth", dateOfBirth);
                }
                else
                {
                    parameters.Add("@DateOfBirth", null);
                }

            }
            updateList = updateList + "ModifiedDate = @ModifiedDate,";
            parameters.Add("@ModifiedDate", DateTime.Now);

            updateList = updateList.Substring(0, updateList.Length - 1);

            // Truy vấn
            var sql = $"UPDATE Customer " +
                $"SET {updateList} " +
                $"WHERE CustomerId = @CustomerId";

            parameters.Add("@CustomerId", customer.CustomerId);

            // Thực hiện truy vấn
            var res = connection.Execute(sql, param: parameters);

            // Trả về kết quả
            return res;
        }

        /// <summary>
        /// Kiểm tra CustomerCode đã tồn tại chưa
        /// </summary>
        /// <param name="customerCode"></param>
        /// <returns>true - đã tồn tại, false - chưa tồn tại</returns>
        public bool CheckCodeIsExist(string customerCode)
        {
            // truy vấn
            string sql = $"SELECT * FROM Customer WHERE CustomerCode = @customerCode";

            // DynamicParameters chống sql injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@customerCode", customerCode);

            // thực hiện truy vấn
            var data = connection.QueryFirstOrDefault<Customer>(sql, param: parameters);

            // nếu không có kết quả => CustomerCode chưa tồn tại, trả về false
            if (data == null)
            {
                return false;
            }

            return true;
        }
    }
}
