using MISA.CUKCUK.Core.DTOs;
using MISA.CUKCUK.Core.DTOs.HelperDTO;
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
    }
}
