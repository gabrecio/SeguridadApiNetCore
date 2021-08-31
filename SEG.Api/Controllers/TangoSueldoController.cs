
using GlobalView;
using SEG.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SEG.Services;

namespace CTRH.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TangoSueldoController : BaseController
    {
        private ITangoSueldoService _tangoService = null;

        /// <summary>
        /// Contructor Tango Controller
        /// </summary>
        /// <param name="tangoService"></param>
        public TangoSueldoController(ITangoSueldoService tangoService)
        {
            this._tangoService = tangoService;
        }


       
        [HttpGet]
        [Route("GetListaDepartamentos")]
        public IActionResult GetListaDepartamentos([FromQuery]PageInfo dPageInfo)
        {
          //  log.Info("GET /api/Tango/GetListaDepartamentos");
            //HttpResponseMessage response;
            var lista = _tangoService.GetListaAgilDepartamentos(dPageInfo.Search, dPageInfo.SortBy);

            if (lista == null)
            {

               // log.Error("ERROR GET api Tango GetListaDepartamentos:  Status 204");
                //response = Request.CreateResponse(HttpStatusCode.NoContent, 0);
                return NoContent();
            }
            else
            {
                //response = Request.CreateResponse(HttpStatusCode.OK, lista);
                return Ok(lista);
            }
        }

    }
}
