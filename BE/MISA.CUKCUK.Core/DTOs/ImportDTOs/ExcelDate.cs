using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs.ImportDTOs
{
    public class ExcelDate
    {
        /// <summary>
        /// chuyển đối tượng sang ngày để import
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Giá trị hiển thị cho người dùng
        /// </summary>
        public string? DateString { get; set; }
    }
}
