using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Exceptions
{
    public class MISAControllerException: Exception
    {
        private string MsgError = string.Empty;
        public string devMsg { get; set; }

        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        /// <param name="userMsg">Thông báo lỗi cho người dùng</param>
        /// <param name="devMsg">Thông báo lỗi cho dev</param>
        public MISAControllerException(string userMsg, string devMsg)
        {
            this.MsgError = userMsg;
            this.devMsg = devMsg;
        }

        public override string Message => this.MsgError;
    }
}
