using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs.CrudDTOs
{
    public class Page<T>
    {
        /// <summary>
        /// Danh sách bản ghi trong 1 trang
        /// </summary>
        public List<T> ListRecord { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        /// Tổng số bản ghi tìm được
        /// </summary>
        public int TotalRecord { get; set; }
    }
}
