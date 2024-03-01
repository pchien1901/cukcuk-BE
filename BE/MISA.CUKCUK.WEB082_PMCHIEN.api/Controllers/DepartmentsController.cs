using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.DTOs.CrudDTOs;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Resources;

namespace MISA.CUKCUK.WEB082_PMCHIEN.api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentsController : BaseController<Department>
    {
        #region Declaration
        IDepartmentRepository _departmentRepository;
        IDepartmentService _departmentService;
        #endregion

        #region Constructor
        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentService departmentService)
        {
            _departmentRepository = departmentRepository;
            _departmentService = departmentService;
        }
        #endregion

        #region Method
        [HttpGet]
        public  override IActionResult Get() 
        {
            try
            {
                var departments = _departmentRepository.Get();
                return StatusCode(200, departments);
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
                var department = _departmentRepository.Get(id);
                return StatusCode(200, department);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public override IActionResult Post([FromBody] Department department)
        {
            try
            {
                var validate = _departmentService.InsertService(department);
                var res = _departmentRepository.Insert(department);
                return StatusCode(201, res);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("{id}")]
        public override IActionResult Put([FromBody] Department department)
        {
            try
            {
                var validate = _departmentService.UpdateService(department);
                if( validate.Success == true )
                {
                    var res = _departmentRepository.Insert(department);
                    return StatusCode(200, res);
                }
                else
                {
                    throw new MISAValidateException(MISAResource.DepartmentDoesNotExist);
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
                var validate = _departmentService.DeleteService(id);
                var res = _departmentRepository.Delete(id);
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
                var res = _departmentRepository.DeleteAny(ids);
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
