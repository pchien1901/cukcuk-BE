using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Exceptions
{/// <summary>
/// Class Exception cho các lỗi 500
/// </summary>
    public class MISAInternalServerErrorException: Exception
    {
        private string MsgError = string.Empty;
        public string devMsg { get; set; }

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="userMsg">Thông báo người dùng</param>
        /// <param name="devMsg">Thông báo cho dev</param>
        public MISAInternalServerErrorException(string userMsg, string devMsg) 
        {
            this.MsgError = userMsg;
            this.devMsg = devMsg;
        }

        public override string Message => this.MsgError;
    }
}
