using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.CUKCUK.Infrastructure.Interfaces;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.MISADatabaseContext
{
    public class MariaDbContext : IMISADbContext
    {
        public IDbConnection Connection { get; }

        public MariaDbContext(IConfiguration config)
        {
            Connection = new MySqlConnection(config.GetConnectionString("Database1"));
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public IEnumerable<T> Get<T>()
        {
            var className = typeof(T).Name;
            // câu truy vấn
            var sql = $"SELECT * FROM {className}";

            // Lấy dữ liệu
            var data = Connection.Query<T>(sql);

            // trả về kết quả
            return data;
        }

        public T? Get<T>(string id)
        {
            var className = typeof(T).Name;
            // Truy vấn
            var sql = $"SELECT * FROM {className} WHERE {className}Id = @id";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", id);

            // Thực hiện truy vấn
            var data = Connection.QueryFirstOrDefault<T>(sql, param: parameters);

            // Trả về kết quả
            return data;
        }

        public int Insert<T>(T entity)
        {
            var className = typeof(T).Name;
            //
            var colNameList = "";
            var colParamList = "";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();

            // lấy properties , id, createdDate, modifiedDate
            var props = typeof(T).GetProperties();
            var id = typeof(T).GetProperty($"{className}Id");
            var createdDate = typeof(T).GetProperty("CreatedDate");
            var modifiedDate = typeof(T).GetProperty("modifiedDate");

            // thêm id cho bản ghi
            id.SetValue(entity, Guid.NewGuid());

            if (createdDate != null)
            {
                // Nếu entity.CreatedDate null thì thêm
                if (createdDate.GetValue(entity) is null)
                {
                    createdDate.SetValue(entity, DateTime.Now);
                }
            }



            // duyệt từng prop
            foreach (var prop in props)
            {
                // lấy tên prop
                var propName = prop.Name;
                // lấy value
                var propValue = prop.GetValue(entity);

                colNameList = colNameList + $"{propName},";
                colParamList = colParamList + $"@{propName},";
                parameters.Add($"@{propName}", propValue);

            }

            colNameList = colNameList.Substring(0, colNameList.Length - 1);
            colParamList = colParamList.Substring(0, colParamList.Length - 1);

            // Truy vấn
            var sql = $"INSERT INTO {className} ({colNameList}) VALUES ({colParamList})";

            // Thực hiện truy vấn
            var res = Connection.Execute(sql, param: parameters);

            // Trả về kết quả
            return res;
        }

        public int Update<T>(T entity)
        {
            var className = typeof(T).Name;

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();

            // lấy properties của T
            var props = typeof(T).GetProperties();

            // tạo danh sách cột và giá trị cập nhật
            var updateList = "";

            // duyệt từng prop
            foreach (var prop in props)
            {
                // lấy tên prop
                var propName = prop.Name;
                if (
                    propName != $"{className}Id"
                    )
                {
                    // lấy value
                    var propValue = prop.GetValue(entity);
                    updateList = updateList + $"{propName} = @{propName},";
                    parameters.Add($"@{propName}", propValue);
                }
            }

            updateList = updateList + "ModifiedDate = @ModifiedDate,";
            parameters.Add("@ModifiedDate", DateTime.Now);

            updateList = updateList.Substring(0, updateList.Length - 1);

            // Truy vấn
            var sql = $"UPDATE {className} " +
                $"SET {updateList} " +
                $"WHERE {className}Id = @id";

            parameters.Add("@id", typeof(T).GetProperty($"{className}Id").GetValue(entity));

            // Thực hiện truy vấn
            var res = Connection.Execute(sql, param: parameters);

            // Trả về kết quả
            return res;
        }

        public int Delete<T>(string id)
        {
            var className = typeof(T).Name;
            // Truy vấn
            var sql = $"DELETE FROM {className} WHERE {className}Id = @id";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@id", id);

            // Thực hiện truy vấn
            var data = Connection.Execute(sql, param: parameters);

            // Trả về kết quả
            return data;
        }

        public int DeleteAny<T>(string[] ids)
        {
            var className = typeof(T).Name;
            var idsArray = "";
            // Truy vấn
            var sql = $"DELETE FROM {className} WHERE {className}Id IN (@ids)";

            // Dùng DynamicParameters chống SQL injection
            DynamicParameters parameters = new DynamicParameters();

            foreach (var item in ids)
            {
                idsArray += $"{item},";
            }
            idsArray = idsArray.Substring(0, idsArray.Length - 1);
            parameters.Add("@ids", idsArray);

            // Thực hiện truy vấn
            var data = Connection.Execute(sql, param: parameters);

            // Trả về kết quả
            return data;
        }
    }
}
