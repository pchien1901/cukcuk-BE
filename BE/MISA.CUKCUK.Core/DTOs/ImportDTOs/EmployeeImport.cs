using MISA.CUKCUK.Core.Const;
using MISA.CUKCUK.Core.CustomValidation;
using MISA.CUKCUK.Core.MISAEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs.ImportDTOs
{
    public class EmployeeImport
    {
        public EmployeeImport()
        {
            this.ImportInvalidErrors = new List<string>();
        }

        /// <summary>
        /// Id của nhân viên
        /// </summary>
        public Guid? EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên (bắt buộc)
        /// </summary>
        /// 
        public string? EmployeeCode { get; set; }

        /// <summary>
        /// Tên nhân viên (bắt buộc)
        /// </summary>
        /// 
        public string? FullName { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        /// 
        public Gender? Gender { get; set; }

        /// <summary>
        /// giới tính - dạng chuỗi
        /// </summary>
        public string? GenderString { get; set; }

        /// <summary>
        /// Quốc tịch
        /// </summary>
        public string?Nationality { get; set; }

        /// <summary>
        /// Dân tộc
        /// </summary>
        public string? Ethnicity { get; set; }

        /// <summary>
        /// Số hợp đồng lao động
        /// </summary>
        public string? ContractNumber { get; set; }

        /// <summary>
        /// Loại hợp đồng lao động
        /// </summary>
        public string? ContractType { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        /// 
        public DateTime? DateOfBirth { get; set; }

        public string? DateOfBirthString { get; set; }

        /// <summary>
        /// id của đơn vị làm việc (Bắt buộc)
        /// </summary>
        /// 
        public Guid? DepartmentId { get; set; }

        /// <summary>
        /// tên đơn vị làm việc (bắt buộc)
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// Chức danh
        /// </summary>
        public Guid? PositionId { get; set; }

        /// <summary>
        /// Tên vị trí
        /// </summary>
        public string? PositionName { get; set; }

        /// <summary>
        /// Số CMTND
        /// </summary>
        public string? IdentityNumber { get; set; }

        /// <summary>
        /// Ngày cấp CMTND
        /// </summary>
        /// 
        public DateTime? IdentityDate { get; set; }

        /// <summary>
        /// Chuỗi ngày từ excel
        /// </summary>
        public string? IdentityDateString { get; set; }


        /// <summary>
        /// Nơi cấp CMTND
        /// </summary>
        public string? IdentityPlace { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Số điện thoại di động
        /// </summary>
        public string? MobilePhoneNumber { get; set; }

        /// <summary>
        /// Số điện thoại cố định
        /// </summary>
        public string? LandlinePhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        /// 
        public string? Email { get; set; }

        /// <summary>
        /// LƯơng
        /// </summary>
        public decimal? Salary { get; set; }

        /// <summary>
        /// Số tài khoản ngân hàng
        /// </summary>
        public string? BankAccountNumber { get; set; }

        /// <summary>
        /// Tên ngân hàng
        /// </summary>
        public string? BankName { get; set; }

        /// <summary>
        /// Chi nhánh
        /// </summary>
        public string? Branch { get; set; }

        /// <summary>
        /// Ngày tham gia
        /// </summary>
        /// 
        public DateTime? JoinDate { get; set; }

        /// <summary>
        /// Là khách hàng
        /// </summary>
        public bool? IsCustomer { get; set; }

        /// <summary>
        /// Là nhà cung cấp
        /// </summary>
        public bool? IsSupplier { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Các thông báo lỗi trả về
        /// </summary>
        public List<string> ImportInvalidErrors { get; set; }

        /// <summary>
        /// Trường thể hiện đã import chưa true - đã import, false - chưa import
        /// </summary>
        public bool? IsImported { get; set; }

        /// <summary>
        /// Trường thể hiện có thể import hay không true - có thể import (validate đúng hết), false - không
        /// </summary>
        public bool? CanImported { get; set; }
    }
}
