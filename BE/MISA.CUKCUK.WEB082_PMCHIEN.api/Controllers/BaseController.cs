using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CUKCUK.Core.DTOs.CrudDTOs;

namespace MISA.CUKCUK.WEB082_PMCHIEN.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<T> : ControllerBase
    {   
        public abstract IActionResult Get();

        public abstract IActionResult Get(string id);

        public abstract IActionResult Post(T entity);

        public abstract IActionResult Put(T entity);

        public abstract IActionResult Delete(string id);

        public abstract IActionResult Delete(string[] ids);
    }
}
