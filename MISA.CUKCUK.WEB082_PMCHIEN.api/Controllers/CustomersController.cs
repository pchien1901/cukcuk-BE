using core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.Entities.EntitiesFromRequest;
using MISA.CUKCUK.Core.Interfaces;
using System.Net;

namespace MISA.CUKCUK.WEB082_PMCHIEN.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        ICustomerRepository _customerRepository;
        ICustomerService _customerService;

        public CustomersController(ICustomerRepository customerRepository, ICustomerService customerService)
        {
            _customerRepository = customerRepository;
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var customers = _customerRepository.Get();
                return StatusCode(200, customers);
            }
            catch (Exception)
            {
                throw ;
            }

        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                var customer = _customerRepository.Get(id);
                return StatusCode(200, customer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Lỗi tại get by id controller");
            }

        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            /*
            var customer = new Customer 
            {
                CustomerId = customerModel.CustomerId,
                CustomerGroupId = customerModel.CustomerGroupId,
                CustomerCode = customerModel.CustomerCode,

            };
            */
            try
            {
                var validate = _customerService.InsertService(customer);
                var res = _customerRepository.Insert(customer);
                return StatusCode(200, res);
            }
            catch (Exception)
            {
                throw ;
            }
    
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody]  Customer customer)
        {
            try
            {
                // kiểm tra đã tồn tại khách hàng chưa
                var validate = _customerService.UpdateService(customer);
                if (validate.Success == true)
                {
                    var res = _customerRepository.Update(customer);
                    return StatusCode(200, res);
                }
                else
                {
                    return StatusCode(400, "Bản ghi chưa tồn tại");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id) 
        {
            try
            {
                var validate = _customerService.DeleteService(id);

                if(validate.Success == true)
                {
                    var res = _customerRepository.Delete(id);
                    return StatusCode(200, res);
                }
                else
                {
                    return StatusCode(400, "Bản ghi không tồn tại");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
