//using core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.DTOs.CrudDTOs;
using MISA.CUKCUK.Core.DTOs.HelperDTO;
using MISA.CUKCUK.Core.DTOs.ImportDTOs;
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
        IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public EmployeesController(IEmployeeRepository employeeRepository, IEmployeeService employeeService, IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Method
        #region Get
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

        [HttpGet("paging")]
        public IActionResult GetByPage(
            [FromQuery(Name = "page")] int page = 1,
            [FromQuery(Name = "size")] int size = 10,
            [FromQuery(Name = "text")] string? text = null
            )
        {
            try
            {
                var searchText = "";
                if(text != null)
                {
                    searchText = text;
                }
                var result = _employeeService.PageService(page, size, searchText);
                if(result.Success)
                {
                    return StatusCode(200, result.DataObject);
                }
                else
                {
                    var devMsg = "Lỗi tại employee controller, GetByPage";
                    throw new MISAControllerException(MISAResource.BaseError, devMsg);
                }
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

        [HttpGet("information")]
        public IActionResult GetEmployeeInfo()
        {
            try
            {
                var res = _unitOfWork.Employees.GetEmployeeInfo();
                return StatusCode(200, res);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion


        #region Post
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


        [HttpPost("validation-import")]
        public IActionResult ValidateImport(IFormFile fileImport)
        {
            try
            {
                var res = _employeeService.ValidateImportService(fileImport);
                return StatusCode(200, res);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("import")]
        public IActionResult ImportEmployeeFromFile(List<EmployeeImport> employeeList)
        {
            try
            {
                var res = _employeeService.ImportEmployee(employeeList);
                if(res.Success)
                {
                    return StatusCode(201, res.Data);
                }
                else
                {
                    var devMsg = "Có lỗi tại ImportEmployeeFromFile - EmployeeControler";
                    throw new MISAControllerException(MISAResource.BaseError, devMsg);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region put
        [HttpPut("{id}")]
        public override IActionResult Put([FromBody] Employee employee)
        {
            try
            {
                var validate = _employeeService.UpdateService(employee);
                if(validate.Success == true)
                {
                    return StatusCode(200, validate.Data);
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
        #endregion

        #region delete
        [HttpDelete("{id}")]
        public override IActionResult Delete(string id)
        {
            try
            {
                var validate = _employeeService.DeleteService(id);
                if(validate.Success)
                {
                    return StatusCode(200, validate.Data);
                }
                else
                {
                    return StatusCode(200, 0);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("batch-deletion")]
        public override IActionResult Delete([FromBody] string[] ids)
        {
            try
            {
                var res = _unitOfWork.Employees.DeleteAny(ids);
                return StatusCode(200, res);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #endregion
    }
}
