using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs.CrudDTOs
{
    public class DeleteAny
    {
        /// <summary>
        /// Mảng các id cần xóa
        /// </summary>
        public List<string> Ids { get; set; }
    }
}
