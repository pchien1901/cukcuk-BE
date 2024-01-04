using MISA.CUKCUK.Core.CustomValidation;
using MISA.CUKCUK.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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
        [MISARequired(ErrorMessage = "Mã khách hàng không được để trống")]
        public string CustomerCode { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [MISARequired(ErrorMessage = "Email không được để trống")]
        [MISAEmailValidate(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [MISAPhoneNumberValidate(ErrorMessage = "Số điện thoại không đúng định dạng")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [MISAGenderValidate(ErrorMessage = "Giới tính phải thuộc từ 0 - 2")]
        public int? Gender { get; set; }

        /// <summary>
        /// Họ tên
        /// </summary>
        [MISARequired(ErrorMessage = "Họ và tên không được để trống")]
        public string FullName { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [JsonConverter(typeof(NullableDateTimeConverter))]
        [MISADateLessThanToday(ErrorMessage = "Ngày sinh không hợp lệ")]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số tiền nợ
        /// </summary>
        [MISACurrencyValidate(ErrorMessage = "Số tiền nợ không được âm")]
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
        /// 
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người sửa đổi
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// ngày sửa đổi
        /// </summary>
        /// 
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? ModifiedDate { get; set; }
    }
}
