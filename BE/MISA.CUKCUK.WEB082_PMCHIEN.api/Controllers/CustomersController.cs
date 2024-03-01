using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.DTOs.CrudDTOs;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Resources;
using System.Net;

namespace MISA.CUKCUK.WEB082_PMCHIEN.api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : BaseController<Customer>
    {
        #region Declaration
        ICustomerRepository _customerRepository;
        ICustomerService _customerService;
        #endregion

        #region Constructor
        public CustomersController(ICustomerRepository customerRepository, ICustomerService customerService)
        {
            _customerRepository = customerRepository;
            _customerService = customerService;
        }
        #endregion

        #region Method
        [HttpGet]
        public override IActionResult Get()
        {
            try
            {
                var customers = _customerRepository.Get();
                return StatusCode(200, customers);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet("{id}")]
        public override IActionResult Get(string id)
        {
            try
            {
                var customer = _customerRepository.Get(id);
                return StatusCode(200, customer);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public override IActionResult Post([FromBody] Customer customer)
        {
            try
            {
                var validate = _customerService.InsertService(customer);
                if (validate.Success == true)
                {
                    return StatusCode(201, validate.Data);
                }
                else
                {
                    var devMsg = "Lỗi tại post Customer";
                    throw new MISAControllerException(MISAResource.BaseError, devMsg);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPut("{id}")]
        public override IActionResult Put([FromBody] Customer customer)
        {
            try
            {
                // kiểm tra đã tồn tại khách hàng chưa
                var validate = _customerService.UpdateService(customer);
                if (validate.Success == true)
                {
                    return StatusCode(200, validate.Data);
                }
                else
                {
                    throw new MISAValidateException(MISAResource.CustomerDoesNotExist);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("{id}")]
        public override IActionResult Delete(string id)
        {
            try
            {
                var validate = _customerService.DeleteService(id);
                var res = _customerRepository.Delete(id);
                return StatusCode(200, res);

                /*
                if (validate.Success == true)
                {
                    var res = _customerRepository.Delete(id);
                    return StatusCode(200, res);
                }
                else
                {
                    throw new MISAValidateException("Khách hàng không tồn tại trong hệ thống.");
                }
                */
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public override IActionResult Delete([FromBody] string[] ids)
        {
            try
            {
                var res = _customerRepository.DeleteAny(ids);
                return StatusCode(200, res);
            }
            catch (Exception)
            {

                throw;
            }
        } 

        #endregion
    }
}
