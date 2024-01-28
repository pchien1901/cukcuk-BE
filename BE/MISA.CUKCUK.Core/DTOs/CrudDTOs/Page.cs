using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.DTOs.CrudDTOs
{
    public class Page<T>
    {
        public List<T> ListRecord { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPage { get; set; }
    }
}
