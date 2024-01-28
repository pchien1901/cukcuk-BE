using Microsoft.AspNetCore.Http;
using MISA.CUKCUK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.CUKCUK.Core.Interfaces
{
    public interface ICustomerService : IBaseService<Customer>
    {
        /// <summary>
        /// xử lí việc nhập file
        /// </summary>
        /// <param name="excel">tệp cần thực hiện xuất khẩu</param>
        /// <returns>object chứa thong tin có thỏa mãn không</returns>
        object ImportService(IFormFile excel);
    }
}
