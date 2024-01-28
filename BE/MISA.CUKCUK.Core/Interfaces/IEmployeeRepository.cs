//using core.Entities;
using MISA.CUKCUK.Core.DTOs.CrudDTOs;
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

        /// <summary>
        /// Lấy thông tin Employee, gồm cả DepartmentName và PositionName
        /// </summary>
        /// <returns>Danh sách EmployeeInfo</returns>
        /// Created by: PMChien
        List<EmployeeInfo> GetEmployeeInfo();

        /// <summary>
        /// Lấy số lượng trang dựa vào kích thước bản ghi 1 trang và từ khóa tìm kiếm
        /// </summary>
        /// <typeparam name="EmployeeInfo"></typeparam>
        /// <param name="pageSize">số lượng bản ghi 1 trang</param>
        /// <param name="text">Từ khóa tìm kiếm theo tên hoặc theo Mã nhân viên</param>
        /// <returns>Số lượng trang</returns>
        /// Created By: PMChien
        int GetPageCount<EmployeeInfo>(int pageSize, string? text);

        /// <summary>
        /// Hàm lấy EmployeeInfo theo trang
        /// </summary>
        /// <param name="offset">vị trí bắt đầu lấy bản ghi</param>
        /// <param name="pageSize">Số bản ghi / trang</param>
        /// <param name="text">Nội dung tìm kiếm theo tên hoặc mã nhân viên</param>
        /// <returns>pageSize bản ghi bắt đầu từ offset</returns>
        /// Created by: PMChien
        List<EmployeeInfo> GetEmployeeInfoByPage(int offset, int pageSize, string? text);
    }
}
