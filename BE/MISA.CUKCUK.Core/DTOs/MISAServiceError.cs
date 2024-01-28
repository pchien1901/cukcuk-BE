using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs
{
    public class MISAServiceError
    {
        /// <summary>
        /// Loại lỗi
        /// </summary>
        public string? Type { get; set; }

        /// <summary>
        /// Tên lỗi
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Mã lỗi
        /// </summary>
        public HttpStatusCode Status { get; set; }

        public string? TraceId { get; set; }

        /// <summary>
        /// trường dữ liệu không hợp lệ
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; set; }


    }
}
