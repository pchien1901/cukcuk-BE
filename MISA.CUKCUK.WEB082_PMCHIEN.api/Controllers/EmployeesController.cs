using core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.DTOs.HelperDTO;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Resources;
using MISA.CUKCUK.Core.Services;

namespace MISA.CUKCUK.WEB082_PMCHIEN.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeesController : BaseController<Employee>
    {
        #region Declaration
        IEmployeeRepository _employeeRepository;
        IEmployeeService _employeeService;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }
        #endregion

        #region Method
        [HttpGet]
        public override IActionResult Get()
        {
            try
            {
                var employees = _employeeRepository.Get();
                return StatusCode(200, employees);
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
                var employee = _employeeRepository.Get(id);
                return StatusCode(200, employee);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("NewCode")]
        public IActionResult GetNewCode()
        {
            try
            {
                var res = _employeeService.MaxCode();
                return StatusCode(200, res);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("validation-code")]
        public IActionResult CheckCodeBeforCU([FromBody] CheckEmployeeCode checkEmployeeCode)
        {
            try
            {
                var res = _employeeService.CheckEmployeeCodeBeforeCU(checkEmployeeCode);
                return StatusCode(200, res);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public override IActionResult Post([FromBody] Employee employee)
        {
            try
            {
                var validate = _employeeService.InsertService(employee);
                if (validate.Success == true)
                {
                    return StatusCode(201, validate.Data);
                }
                else
                {
                    var devMsg = "Lỗi tại post Employee";
                    throw new MISAControllerException(MISAResource.BaseError, devMsg);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPut("{id}")]
        public override IActionResult Put([FromBody] Employee employee)
        {
            try
            {
                var validate = _employeeService.UpdateService(employee);
                if(validate.Success == true)
                {
                    var res = _employeeRepository.Update(employee);
                    return StatusCode(200, res);
                }
                else
                {
                    throw new MISAValidateException(MISAResource.EmployeeDoesNotExist);
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
                var validate = _employeeService.DeleteService(id);
                var res = _employeeRepository.Delete(id);
                return StatusCode(200, res);
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
                var res = _employeeRepository.DeleteAny(ids);
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
