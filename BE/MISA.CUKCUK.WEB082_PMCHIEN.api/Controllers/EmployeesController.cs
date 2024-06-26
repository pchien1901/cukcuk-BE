﻿//using core.Entities;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

        [HttpGet("export-file")]
        public IActionResult ExportEmployeeExportFile(
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
                var result = _employeeService.ExportEmployee(page, size, searchText);

                if (result.Success && result.DataObject != null)
                {
                    var excelData = (Dictionary<string, object>)result.DataObject;
                    var fileBytes = (byte[])excelData["FileBytes"];
                    var fileName = (string)excelData["FileName"];

                    return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName, true);
                }
                else
                {
                    return StatusCode(500, new
                    {
                        devMsg = MISAResource.ExportFileFail,
                        userMsg = MISAResource.ExportFileFail,
                        errorCode = "",
                        moreInfor = "",
                        traceId = ""
                    });
                }
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
                var res = _employeeService.ReadImportFile(fileImport);
                if(res.Success)
                {
                    return StatusCode(200, res.DataObject);
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

        [HttpPost("{keyImport}")]
        public IActionResult ImportEmployeeFromFile(string keyImport)
        {
            try
            {
                var res = _employeeService.ImportEmployee(keyImport);
                if(res.Success)
                {
                    return StatusCode(201, res.DataObject);
                }
                else
                {
                    return StatusCode(500, new { 
                        devMsg = MISAResource.ImportFileFail,
                        userMsg = MISAResource.ImportFileFail,
                        errorCode = "",
                        moreInfor = "",
                        traceId = "",
                        data = res.DataObject
                    });
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

        [HttpDelete]
        public override IActionResult Delete( string[] ids)
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
