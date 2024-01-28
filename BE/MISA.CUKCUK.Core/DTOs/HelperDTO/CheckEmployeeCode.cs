using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs.HelperDTO
{
    /// <summary>
    /// Class lưu dữ liệu gửi lên để kiểm tra mã nhân viên đã tồn tại trong Db chưa
    /// Vơi trường hợp Create và Update
    /// </summary>
    public class CheckEmployeeCode
    {
        /// <summary>
        /// id của nhân viên
        /// </summary>
        public string? EmployeeId { get; set; }

        /// <summary>
        /// Mã nhân viên
        /// </summary>
        public string? EmployeeCode { get; set; }

        /// <summary>
        /// Có phải đang Create không
        /// </summary>
        public bool?   IsCreate { get; set;}

        /// <summary>
        /// Có phải đang Update không
        /// </summary>
        public bool? IsUpdate { get; set;}
    }
}
