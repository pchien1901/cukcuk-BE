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
        /// Created By: PMCHIEN
        List<T> Get();

        /// <summary>
        /// Lấy theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Bản ghi có id trùng khớp</returns>
        /// Created By: PMCHIEN
        T Get(string id);

        /// <summary>
        /// Thêm bản ghi
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Số bản ghi được chèn vào DB</returns>
        /// Created By: PMCHIEN
        int Insert(T entity);

        /// <summary>
        /// Sửa bản ghi
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Created By: PMCHIEN
        int Update(T entity);

        /// <summary>
        /// Xóa bản ghi theo Id
        /// </summary>
        /// <param name="customer"></param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Created By: PMCHIEN
        int Delete(string id);

        /// <summary>
        /// Xóa tất cả bản ghi
        /// </summary>
        /// <returns></returns>
        /// Created By: PMCHIEN
        int DeleteAll();

       
    }
}
