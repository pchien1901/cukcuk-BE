using Microsoft.VisualBasic;
using MISA.CUKCUK.Core.Const;
using MISA.CUKCUK.Core.CustomValidation;
using MISA.CUKCUK.Core.MISAEnum;
using MISA.CUKCUK.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Entities
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
        [MISARequired(ErrorMessage = MISAConst.ERROR_CUSTOMERCODE_REQUIRED)]
        public string CustomerCode { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [MISARequired(ErrorMessage = MISAConst.ERROR_EMAIL_REQUIRED)]
        [MISAEmailValidate(ErrorMessage = MISAConst.ERROR_EMAIL_INVALID)]
        public string Email { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        [MISAPhoneNumberValidate(ErrorMessage = MISAConst.ERROR_PHONENUMBER_INVALID)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [MISAGenderValidate(ErrorMessage = MISAConst.ERROR_GENDER_INVALID)]
        public Gender? Gender { get; set; }

        /// <summary>
        /// Họ tên
        /// </summary>
        [MISARequired(ErrorMessage = MISAConst.ERROR_FULLNAME_REQUIRED)]
        public string FullName { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        [JsonConverter(typeof(NullableDateTimeConverter))]
        [MISADateLessThanToday(ErrorMessage = MISAConst.ERROR_DATEOFBIRTH_INVALID)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Số tiền nợ
        /// </summary>
        [MISACurrencyValidate(ErrorMessage = MISAConst.ERROR_DEBITAMOUT_IS_NOT_NEGATIVE)]
        public decimal? DebitAmount { get; set; }

        /// <summary>
        /// Tên công ty
        /// </summary>
        public string? CompanyName { get; set; }

        /// <summary>
        /// Mã số thuế
        /// </summary>
        public string? CompanyTaxCode { get; set; }

        /// <summary>
        /// Mã thành viên
        /// </summary>
        public string? MemberCarCode { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string? Note { get; set; }

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
