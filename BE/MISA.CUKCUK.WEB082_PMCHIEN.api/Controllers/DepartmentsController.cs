using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.Auth;
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
        IUnitOfWork _unitOfWork;
        #endregion

        #region Constructor
        public DepartmentsController(IDepartmentRepository departmentRepository, IDepartmentService departmentService, IUnitOfWork unitOfWork)
        {
            _departmentRepository = departmentRepository;
            _departmentService = departmentService;
            _unitOfWork = unitOfWork;
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

        [Authorize(Roles = UserRoles.Admin)]
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id}")]
        public override IActionResult Put([FromBody] Department department)
        {
            try
            {
                var validate = _departmentService.UpdateService(department);
                if( validate.Success == true )
                {
                    //var res = _departmentRepository.Insert(department);
                    return StatusCode(200, validate.Data);
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public override IActionResult Delete(string id)
        {
            try
            {
                var validate = _departmentService.DeleteService(id);
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

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete]
        public override IActionResult Delete([FromBody] string[] ids)
        {
            try
            {
                var res = _unitOfWork.Departments.DeleteAny(ids);
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
