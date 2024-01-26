using core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        /// <summary>
        /// Lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách các bản ghi</returns>
        /// Created By: PMCHIEN (31/12/2023)
        IEnumerable<T> Get();

        /// <summary>
        /// Lấy theo Id
        /// </summary>
        /// <param name="id">Id của bản ghi</param>
        /// <returns>Bản ghi có id trùng khớp</returns>
        /// Created By: PMCHIEN (31/12/2023)
        T Get(string id);

        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm vào Database</param>
        /// <returns>Số bản ghi được chèn vào Database</returns>
        /// Created By: PMCHIEN (31/12/2023)
        int Insert(T entity);

        /// <summary>
        /// Sửa bản ghi
        /// </summary>
        /// <param name="entity">Đối tượng cần chỉnh sửa vào Database</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Created By: PMCHIEN (31/12/2023)
        int Update(T entity);

        /// <summary>
        /// Xóa bản ghi theo Id
        /// </summary>
        /// <param name="customer">id bản ghi cần xóa</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Created By: PMCHIEN (31/12/2023)
        int Delete(string id);

        /// <summary>
        /// Xóa danh sách bản ghi
        /// </summary>
        /// <param name="ids">mảng id cần xóa</param>
        /// <returns>Số bản ghi bị thay đổi</returns>
        /// created by: PMCHIEN  (31/12/2023)
        int DeleteAny(string[] ids);

       
    }
}
