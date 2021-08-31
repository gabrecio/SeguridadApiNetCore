using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SEG.Services;

namespace SEG.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PresupuestoController : BaseController
    {
        private IPresupuestoService _presupuestoService = null;
        
        public PresupuestoController(IPresupuestoService presupuestoService)
        {
            this._presupuestoService = presupuestoService;
        }



       
        [HttpGet]
        [Route("GetListaCentroCosto")]
        public IActionResult GetListaCentroCosto()
        {
            //log.Info("GET /api/Presupuesto/GetListaCentroCosto");
            //HttpResponseMessage response;
            //hacer el check de usuario y pass
            var resp = _presupuestoService.GetCentroscosto();

            if (resp == null)
            {

             //   log.Error("ERROR GET api Presupuesto GetListaCentroCosto:  Status 204");
                //response = Request.CreateResponse(HttpStatusCode.NoContent, 0);
                return NoContent();
            }
            else
            {
                //response = Request.CreateResponse(HttpStatusCode.OK, resp);
                return Ok(resp);
            }
        }
    }
}
