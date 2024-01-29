using Microsoft.AspNetCore.Http;
using MISA.CUKCUK.Core.DTOs;
using MISA.CUKCUK.Core.DTOs.HelperDTO;
using MISA.CUKCUK.Core.DTOs.ImportDTOs;
using MISA.CUKCUK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Interfaces
{
    public interface IEmployeeService: IBaseService<Employee>
    {
        /// <summary>
        /// Tạo giá trị EmployeeCode lớn nhất
        /// </summary>
        /// <returns>new EmployeeCode</returns>
        /// Created by: PMChien
        public string MaxCode();

        /// <summary>
        /// Kiểm tra EmployeeCode trước khi thực hiện Insert hoặc Update tại Frontend
        /// </summary>
        /// <param name="employee">Đối tượng cần kiểm tra</param>
        /// <returns>true - đã tồn tại, false - chưa tồn tại</returns>
        /// Created by: PMChien
        bool CheckEmployeeCodeBeforeCU(CheckEmployeeCode checkEmployeeCode);

        /// <summary>
        /// Lấy EmployeeInfo theo page
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng bản ghi trên 1 trang</param>
        /// <param name="key">Từ khóa tìm kiếm</param>
        /// <returns>MISAServiceResul</returns>
        /// Created by: PMChien
        MISAServiceResult PageService(int page, int pageSize, string? key);

        /// <summary>
        /// Hàm kiểm tra tệp excel và dữ liệu trong tệp
        /// </summary>
        /// <param name="fileImport">file để import</param>
        /// <returns>danh sách EmployeeImport chứa thông tin đã được validate</returns>
        /// Created by: PMChien
        IEnumerable<EmployeeImport> ValidateImportService(IFormFile fileImport);

        /// <summary>
        /// Hàm lưu các giá trị EmployeeImport được validate từ trước vào database
        /// </summary>
        /// <param name="employeeList">danh sách EmployeeList đã được trả về cho frontend từ ValidateImportService</param>
        /// <returns>
        /// MISAServiceResult {
        ///     Success = true - thêm thành công,
        ///     Data = số lượng bản ghi thay đổi
        /// }
        /// </returns>
        /// Created by: PMChien
        MISAServiceResult ImportEmployee(List<EmployeeImport> employeeList);
    }
}
