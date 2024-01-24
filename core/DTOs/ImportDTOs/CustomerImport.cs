using core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs.ImportDTOs
{
    public class CustomerImport: Customer
    {
        public CustomerImport()
        {
            this.ImportInvalidErrors = new List<string>();
        }

        /// <summary>
        /// Tên nhóm khách hàng
        /// </summary>
        public string? CustomerGroupName { get; set; }

        /// <summary>
        /// Các thông báo lỗi trả về
        /// </summary>
        public List<string> ImportInvalidErrors { get; set; }

        /// <summary>
        /// Trường thể hiện đã import chưa true - đã import, false - chưa import
        /// </summary>
        public bool? IsImported { get; set; }

        /// <summary>
        /// Trường thể hiện có thể import hay không true - có thể import (validate đúng hết), false - không
        /// </summary>
        public bool? CanImported { get; set; }
    }
}
