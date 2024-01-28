using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs
{
    public class MISAErrorResponse
    {
        /// <summary>
        /// Thông báo cho dev
        /// </summary>
        public string devMsg { get; set; }

        /// <summary>
        /// Thông báo cho user
        /// </summary>
        public string userMsg { get; set; }

        /// <summary>
        /// Mã lỗi nội bộ
        /// </summary>
        public string errorCode { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// </summary>
        public string moreInfor { get; set; }

        /// <summary>
        /// Id tra cứu
        /// </summary>
        public string traceId { get; set; }
    }
}
