using MISA.CUKCUK.Core.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.Entities
{
    public class Customer
    {
        /// <summary>
        /// Id khách hàng
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Id nhóm khách hàng
        /// </summary>
        public Guid? CustomerGroupId { get; set; }

        /// <summary>
        /// Mã khách hàng
        /// </summary>
        [Required(ErrorMessage = "Mã khách hàng không được để trống")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [RegularExpression(pattern: "^\\d+$", ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [Range(minimum: 0, maximum: 2, ErrorMessage = "Giới tính phải thuộc từ 0 - 2")]
        public int? Gender { get; set; }

        /// <summary>
        /// Họ tên
        /// </summary>
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string FullName { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [DateLessThanToday(ErrorMessage = "Ngày sinh không hợp lệ")]
        public string? DateOfBirth { get; set; }

        /// <summary>
        /// Số tiền nợ
        /// </summary>
        [Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "Số tiền nợ không được âm")]
        public decimal? DebitAmount { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string? CompanyName { get; set; }

        /*
        public string? Identity { get; set; }

        public string? IdentityPlace { get; set; }

        public DateTime? IdentityDate { get; set; }

        */

        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public string? CreatedDate
        {
            get;
            set; 
        }

        /// <summary>
        /// Người sửa đổi
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// ngày sửa đổi
        /// </summary>
        public string? ModifiedDate { get; set; }
    }
}
