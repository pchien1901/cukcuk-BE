//using core.Entities;
using MISA.CUKCUK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Interfaces
{
    public interface IDepartmentRepository: IBaseRepository<Department>
    {
        /// <summary>
        /// Kiểm tra Code đã tồn tại chưa
        /// </summary>
        /// <param name="code">DepartmentCode cần kiểm tra</param>
        /// <returns>true - code đã tồn tại, false - code chưa tồn tại</returns>
        /// Created By: PMCHIEN
        bool CheckCodeIsExist(string code);

        /// <summary>
        /// Lấy tất cả đơn vị theo code
        /// </summary>
        /// <param name="code">DepartmentCode cần lấy danh sách</param>
        /// <returns>Danh sách các bản ghi</returns>
        /// Created By: PMCHIEN
        List<Department> GetByCode(string code);
    }
}
