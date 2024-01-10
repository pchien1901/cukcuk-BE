using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.Entities;
using MISA.CUKCUK.Core.Exceptions;
using MISA.CUKCUK.Core.Interfaces;
using MISA.CUKCUK.Core.Resources;

namespace MISA.CUKCUK.WEB082_PMCHIEN.api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PositionsController : BaseController<Position>
    {
        #region Declaration
        IPositionRepository positionRepository;
        IPositionService positionService;
        #endregion

        #region Constructor
        public PositionsController(IPositionService positionService, IPositionRepository positionRepository)
        {
            this.positionService = positionService;
            this.positionRepository = positionRepository;
        }
        #endregion

        #region Method
        [HttpGet]
        public override IActionResult Get()
        {
            try
            {
                var positions = positionRepository.Get();
                return StatusCode(200, positions);
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
                var position = positionRepository.Get(id);
                return StatusCode(200, position);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public override IActionResult Post([FromBody] Position position)
        {
            try
            {
                var validate = positionService.InsertService(position);
                if(validate.Success == true)
                {
                    return StatusCode(201, validate.Data);
                }
                else
                {
                    var devMsg = "lỗi tại post Position";
                    throw new MISAControllerException(MISAResource.BaseError, devMsg);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [HttpPut("{id}")]
        public override IActionResult Put( [FromBody] Position position)
        {
            try
            {
                var validate = positionService.UpdateService(position);
                if(validate.Success == true)
                {
                    return StatusCode(200, validate.Data);
                }
                else
                {
                    throw new MISAValidateException(MISAResource.PositionDoesNotExist);
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
                var res = positionRepository.Delete(id);
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
                var res = positionRepository.DeleteAny(ids);
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
