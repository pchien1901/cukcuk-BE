using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.DTOs;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using System.Net;

namespace MISA.CUKCUK.WEB082_PMCHIEN.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerGroupsController : ControllerBase
    {
        ICustomerGroupRepository _customerGroupRepository;
        ICustomerGroupService _customerGroupService;

        public CustomerGroupsController(ICustomerGroupRepository customerGroupRepository, ICustomerGroupService customerGroupService)
        {
            _customerGroupRepository = customerGroupRepository;
            _customerGroupService = customerGroupService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var data = _customerGroupRepository.Get();
                return StatusCode(200, data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                var data = _customerGroupRepository.Get(id);
                return StatusCode(200, data);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CustomerGroup customerGroup)
        {
            try
            {
                var validate = _customerGroupService.InsertService(customerGroup);
                var res = _customerGroupRepository.Insert(customerGroup);
                return StatusCode(201, res);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] CustomerGroup customerGroup)
        {
            try
            {
                var validate = _customerGroupService.UpdateService(customerGroup);
                if (validate.Success = true)
                {
                    var res = _customerGroupRepository.Update(customerGroup);
                    return StatusCode(200, res);
                }
                else
                {
                    throw new MISAValidateException("Nhóm khách hàng không tồn tại trong hệ thống.");
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
                var customerGroupName = _customerGroupRepository.Get(id).CustomerGroupName;
                var res = _customerGroupRepository.Delete(id);
                if (res is int)
                {
                    return StatusCode(200, res);
                }
                else
                {
                    throw new MISACanNotDeleteForeignField($"Không thể xóa nhóm {customerGroupName} vì có nhiều khách hàng thuộc nhóm này", "CustomerGroup");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
