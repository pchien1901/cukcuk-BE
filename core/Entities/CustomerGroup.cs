using MISA.CUKCUK.Core.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Entities
{
    public class CustomerGroup
    {
        /// <summary>
        /// Id của trường
        /// </summary>
        public Guid CustomerGroupId { get; set; }

        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        [MISARequired(ErrorMessage = "Tên nhóm khách hàng không được bỏ trống")]
        public string CustomerGroupName { get; set; }

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
        /// Người chỉnh sửa
        /// </summary>
        public string? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        /// 
        [JsonConverter(typeof(NullableDateTimeConverter))]
        public DateTime? ModifiedDate { get; set; }
    }
}
