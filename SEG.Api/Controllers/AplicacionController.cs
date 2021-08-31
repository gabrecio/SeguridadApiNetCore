using GlobalView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SEG.API.Controllers;
using SEG.LocalViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using SEG.Services;

namespace SEG.API.Contaplicacionlers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AplicacionController : BaseController
    {
        private IAplicacionService _appService = null;
        public AplicacionController(IAplicacionService appService)
        {
            this._appService = appService;
        }

      
       
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           // log.Info("GET api/ aplicacion/" + id);
            
            var registro = _appService.GetById(id);
            if (registro == null)
            {
                //log.Error("ERROR GET api /  aplicacion:  Status 404");                
                return NotFound(0);
            }            
            return Ok(registro);
        }

        [AllowAnonymous]
        [HttpGet]
        public List<AplicacionViewModel> Get()
        {
            //log.Info("GET api/aplicacion");
            List<AplicacionViewModel> lista = _appService.GetAll("").ToList();
            return lista;
        }
      
        [HttpPost("{id}")]
        public IActionResult Post(AplicacionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
               // log.Error("ERROR POST api / aplicacion: Model Invalid");
                return BadRequest(ModelState);
            }
            //log.Info("POST api/ aplicacion * id:" + viewModel.Id);
            var result = _appService.Insert(viewModel);
            if (result == 0)
                return BadRequest("Error al intentar guardar el nuevo registro");
            else
                return Ok(new GenericResponse { Status = 200, Message = result.ToString() });

        }
      
        [HttpPut("{id}")]
        public IActionResult Put(AplicacionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
               // log.Error("ERROR PUT api / aplicacion: Model Invalid");
                return BadRequest(ModelState);
            }
            try
            {
                _appService.Update(viewModel);
            }
            catch (DbUpdateConcurrencyException e)
            {
               // log.Error("ERROR PUT api/aplicacion: " + e.Message);
                return NotFound();
            }
            return Ok(new GenericResponse { Status = 200, Message = "El registro fue actualizado satisfactoriamente." });
        }
      
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //log.Info("DELETE api/ aplicacion * id: " + id);
            try
            {
                if (_appService.Delete(id))
                    return Ok(new GenericResponse { Status = 200, Message = "El registro fue eliminado correctamente." });
                else
                    return Ok(new GenericResponse { Status = 203, Message = "No se puede eliminar, tiene registros asociados" });
            }
            catch (Exception e)
            {
                return Ok(new GenericResponse { Status = 203, Message = "No se puede eliminar, tiene registros asociados" });
            }
        }

      
        [HttpGet]
        [Route("Search")]        
        public IActionResult Search([FromQuery] PageInfo dPageInfo)
        {

           // log.Info("GET /api/aplicacion/Search");
            var userResp = new GenericListResponse();
            List<AplicacionViewModel> lista;

            lista = _appService.GetAll(dPageInfo.Search).ToList();

            userResp.Total = lista.Count();
            userResp.Lista = lista.Skip((dPageInfo.Page - 1) * dPageInfo.ItemsPerPage).Take(dPageInfo.ItemsPerPage).ToList();

            // Write the list to the response body.
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, userResp);
            return Ok(userResp);
        }

    }
}
