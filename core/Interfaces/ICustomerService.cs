using core.Entities;
using Microsoft.AspNetCore.Http;
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
        /// xử lí việc nhập filet
        /// </summary>
        /// <param name="excel"></param>
        /// <returns></returns>
        object ImportService(IFormFile excel);
    }
}
