using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs
{
    /// <summary>
    /// Class thông tin trả về cho client
    /// </summary>
    public class MISAResponse
    {
        /// <summary>
        /// Thể hiện sự thành công/ thất bại của hành động
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// Mã trả về, ở đây là status code
        /// </summary>
        public HttpStatusCode Code { get; set; }
        /// <summary>
        /// develop message
        /// </summary>
        public string DevMsg { get; set; }
        /// <summary>
        /// user message
        /// </summary>
        public string UserMsg { get; set; }
    }
}
