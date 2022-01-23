/************************************************************************************************************
 * Descripción: Controlador Entidad CentroCosto. 
 * Observaciones: 
 * Creado por: Gabriel Recio.
 * Historial de Revisiones: 
 * ----------------------------------------------------------------------------------------------- 
 * Fecha        Evento / Método Autor   Descripción 
 * ----------------------------------------------------------------------------------------------- 
 * 04/10/2017 13:58		Implementación inicial. 
 * ----------------------------------------------------------------------------------------------- 
*/ 
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
//using System.Web.Http;
using GlobalView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SEG.LocalViewModels;
using SEG.Services;

namespace SEG.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CentroCostoController : BaseController
    { 
       
    private ICentroCostoService _CentroCostoService = null; 
    public CentroCostoController(ICentroCostoService CentroCostoService) 
    { 
       this._CentroCostoService = CentroCostoService; 
    }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
    { 
      //  log.Info("GET api/ CentroCosto/" +  id);
        //HttpResponseMessage response;
        var registro = _CentroCostoService.GetById(id); 
        if (registro == null) 
        {
          //  log.Error("ERROR GET api /  CentroCosto:  Status 404");
            return NotFound(0);// Request.CreateResponse(HttpStatusCode.NotFound, 0);
             
        } 
        
        return Ok(registro); 
    }
   
    [HttpGet]
    public List<CentroCostoViewModel> Get()
    { 
       // log.Info("GET api/CentroCosto");
        List<CentroCostoViewModel>  lista =  _CentroCostoService.GetAll("").ToList();
        return lista; 
    } 
        /// <summary>
        /// Insertar centro de costo
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
   
   
    [HttpGet]
    [Route("Search")]
    public IActionResult Search([FromQuery]PageInfo dPageInfo)
    { 
          //  log.Info("GET /api/CentroCosto/Search"); 
            var respuesta = new GenericListResponse();
            List<CentroCostoViewModel> entidadLista;
            entidadLista = _CentroCostoService.GetAll(dPageInfo.Search).ToList();
            respuesta.Total = entidadLista.Count();
            respuesta.Lista = entidadLista.Skip((dPageInfo.Page - 1) * dPageInfo.ItemsPerPage).Take(dPageInfo.ItemsPerPage).ToList();
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, respuesta);
            return Ok(respuesta);
    }

      
        [HttpGet]
        [Route("GetListaAgil")]
        public IActionResult GetListaAgil()
        {
         //   log.Info("GET /api/CentroCosto/GetListaAgil");
            var respuesta = new GenericListResponse();
            var total = 0;
            respuesta.Lista = _CentroCostoService.GetListaAgil().ToList();
            respuesta.Total = total;
           // HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, respuesta);
            return Ok(respuesta);
        }

        [HttpGet]
        [Route("GetListaAgil2")]
        public IActionResult GetListaAgil2()
        {
            //   log.Info("GET /api/CentroCosto/GetListaAgil");
            /*var respuesta = new GenericListResponse();
            var total = 0;
            respuesta.Lista = _CentroCostoService.GetListaAgil().ToList();
            respuesta.Total = total;*/
            // HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, respuesta);
            return Ok(_CentroCostoService.GetListaAgil().ToList());
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("LimpiarCacheCentroCosto")]
        public IActionResult LimpiarCacheCentroCosto()
        {
           // log.Info("GET api/ usuario/LimpiarCacheCentroCosto");

            var registro = _CentroCostoService.LimpiarCacheCentroCosto();
            if (registro)
            {
                return Ok(new GenericResponse { Status = 200, Message = "ok" });
            }
            return Ok(new GenericResponse { Status = 203, Message = "error" });

        }

    } 
}
