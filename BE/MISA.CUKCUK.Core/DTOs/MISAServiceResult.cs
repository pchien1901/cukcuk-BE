using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs
{
    public class MISAServiceResult
    {
        //public MISAServiceResult()
        //{
            
        //}

        //public MISAServiceResult( bool success, int data)
        //{
        //    Success = success;
        //    Data = data;
        //}

        /// <summary>
        /// true - thành công
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Thông tin thêm 
        /// Số bản ghi được cập nhật trong database
        /// </summary>
        public int? Data { get; set; }

        /// <summary>
        /// Data chứa object
        /// </summary>
        public object? DataObject { get; set; }

        /*
         public HttpStatusCode StatusCode { get; set; }

         public List<string> Errors { get; set; }
        */
        /// <summary>
        /// Các lỗi
        /// </summary>
        public MISAServiceError Error { get; set; }
    }
}
