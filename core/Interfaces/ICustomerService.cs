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

        object ImportService(IFormFile excel);
    }
}
