using MISA.CUKCUK.Core.DTOs.CrudDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Infrastructure.Interfaces
{
     public interface IMISADbContext
    {
        IDbConnection Connection { get; set; }
        IDbTransaction Transaction { get; set; }

        /// <summary>
        /// Đóng kết nối
        /// </summary>
        public void Dispose();

        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng</typeparam>
        /// <returns>Danh sách bản ghi</returns>
        /// Created by: PMCHIEN (03/01/2024)
        public IEnumerable<T> Get<T>();

        /// <summary>
        /// Lấy một bản ghi theo id
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng</typeparam>
        /// <param name="id">id của bản ghi </param>
        /// <returns>entity || null</returns>
        /// Created by: PMCHIEN (03/01/2024)
        public T? Get<T>(string id);

        /// <summary>
        /// Chèn một bản ghi vào DB
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng</typeparam>
        /// <param name="entity">Đối tượng cần thêm vào Database</param>
        /// <returns>Số lượng bản ghi thay đổi</returns>
        /// Created by: PMCHIEN (03/01/2024)
        public int Insert<T>(T entity);

        /// <summary>
        /// Sửa một bản ghi 
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng</typeparam>
        /// <param name="entity">Đối tượng cần chỉnh sửa</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Created by: PMCHIEN (03/01/2024)
        public int Update<T>(T entity);

        /// <summary>
        /// Xóa một bản ghi
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng</typeparam>
        /// <param name="id">Id của bản ghi cần xóa</param>
        /// <returns>Số lượng bản ghi thay đổi</returns>
        /// Created by: PMCHIEN (03/01/2024)
        public int Delete<T>(string id);

        /// <summary>
        /// Xóa danh sách bản ghi
        /// </summary>
        /// <typeparam name="T">Kiểu của đối tượng</typeparam>
        /// <param name="deleteAny">DTO cho việc xóa nhiều bản ghi</param>
        /// <returns>Số lượng bản ghi thay đổi</returns>
        public int DeleteAny<T>(string[] deleteAny);
    }
}
