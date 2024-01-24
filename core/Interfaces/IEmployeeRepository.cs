using core.Entities;
using MISA.CUKCUK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Interfaces
{
    public interface IEmployeeRepository: IBaseRepository<Employee>
    {
        /// <summary>
        /// Kiểm tra Code đã tồn tại chưa
        /// </summary>
        /// <param name="code">EmployeeCode cần kiểm tra</param>
        /// <returns>true - code đã tồn tại, false - code chưa tồn tại</returns>
        /// Created By: PMCHIEN
        bool CheckCodeIsExist(string code);

        /// <summary>
        /// Lấy tất cả nhân viên theo code
        /// </summary>
        /// <param name="code">EmployeeCode cần lấy danh sách</param>
        /// <returns>Danh sách các bản ghi</returns>
        /// Created By: PMCHIEN
        List<Employee> GetByCode(string code);

        /// <summary>
        /// Lấy giá trị EmployeeCode lớn nhất trong DataBase
        /// </summary>
        /// <returns>EmployeeCode lớn nhất</returns>
        /// Created by: PMChien
        string GetMaxCode();
    }
}
