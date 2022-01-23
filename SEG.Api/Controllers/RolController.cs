/************************************************************************************************************
 * Descripción: Controlador Entidad rol. 
 * Observaciones: 
 * Creado por: Gabriel Recio.
 * Historial de Revisiones: 
 * ----------------------------------------------------------------------------------------------- 
 * Fecha        Evento / Método Autor   Descripción 
 * ----------------------------------------------------------------------------------------------- 
 * 04/10/2016 18:24		Implementación inicial. 
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
using SEG.LocalViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SEG.Services;

namespace SEG.API.Controllers
{
    [Authorize]
    [ApiController]
    /// <summary>
    /// Controlador de Roles
    /// </summary>
    [Route("api/[controller]")]
    public class RolController : BaseController
    { 
    private IRolService _rolService = null; 
    public RolController(IRolService rolService) 
    { 
       this._rolService = rolService; 
    }

        /// <summary>
        /// Recupera los datos del rol según id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public IActionResult Get(int id)
    { 
        //log.Info("GET api/ rol/" +  id);
        //HttpResponseMessage response;
        var registro = _rolService.GetById(id); 
        if (registro == null) 
        {
         //   log.Error("ERROR GET api /  rol:  Status 404");
            //response = Request.CreateResponse(HttpStatusCode.NotFound, 0);
            return NotFound(0); 
        } 
        //response = Request.CreateResponse(HttpStatusCode.OK, registro); 
        return Ok(registro); 
    }

   
        [HttpGet]
    public List<RolViewModel> Get()
    { 
        //log.Info("GET api/rol");
        List<RolViewModel>  lista =  _rolService.GetAll("").ToList();
        return lista; 
    } 
    
 
    [HttpPost("{id}")]   
    public IActionResult Post([FromBody]RolViewModel viewModel)
    { 
        if (!ModelState.IsValid)
        { 
            //log.Error("ERROR POST api / rol: Model Invalid");
            return BadRequest(ModelState);
        }  
            var result = _rolService.Insert(viewModel);
            if (result == 0)              
                return BadRequest("Error al intentar guardar el nuevo registro");
            else
                return Ok(new GenericResponse { Status = 200, Message = result.ToString() });
           
    }
        
    [HttpPut("{id}")]
    public IActionResult Put(RolViewModel viewModel)
    { 
        if (!ModelState.IsValid)
        { 
            //log.Error("ERROR PUT api / rol: Model Invalid");
            return BadRequest(ModelState);
        } 
        try
        {
            _rolService.Update(viewModel);
        } 
        catch (DbUpdateConcurrencyException e) 
        {
            //log.Error("ERROR PUT api/rol: " + e.Message);
            return NotFound();
        } 
        return Ok(new GenericResponse { Status = 200, Message = "El registro fue actualizado satisfactoriamente." });
    } 
   
    [HttpDelete("{id}")]
    public IActionResult Delete(int id) 
    {
       //log.Info("DELETE api/ rol * id: " + id);
        try
        {
           if (_rolService.Delete(id)) 
                return Ok(new GenericResponse { Status = 200, Message = "El registro fue eliminado correctamente." });
           else
                return Ok(new GenericResponse { Status = 203, Message = "No se puede eliminar, tiene registros asociados" });
            } 
        catch (Exception e) 
        {
            return Ok(new GenericResponse { Status = 203, Message = "No se puede eliminar, tiene registros asociados" });
        } 
    }


        /// <summary>
        /// Buscar roles, incluye paginación
        /// </summary>
        /// <param name="itemsPerPage"></param>
        /// <param name="page"></param>
        /// <param name="reverse"></param>
        /// <param name="search"></param>
        /// <param name="sortBy"></param>
        /// <param name="totalItems"></param>
        /// <returns></returns>
      
        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string aplicacion,  int itemsPerPage, int page, bool reverse, string search, string sortBy)
        {
            // PageInfo dPageInfo = JsonConvert.DeserializeObject<PageInfo>(pagingInfo);
            PageInfo dPageInfo = new PageInfo()
            {
                Page = page,
                ItemsPerPage = itemsPerPage,
                SortBy = sortBy,
                Reverse = reverse,
                Search = search,
                TotalItems = 0
            };
          //  log.Info("GET /api/rol/Search/{pagingInfo}");
            //var rolResp = new RolResponse();
            var rolResp = new GenericListResponse();
            List<RolViewModel> rolList;
          

            // filtering
            if ((dPageInfo.Search != null && dPageInfo.Search.Trim() != String.Empty))
            {
                rolList = _rolService.GetAll(dPageInfo.Search); //.Where(au => au.Nombre.ToLower().Contains(dPageInfo.search)).ToList();

            }
            else
                rolList = _rolService.GetAll("").OrderBy(x => x.nombre).ToList();

            if (!string.IsNullOrEmpty(aplicacion))
                rolList = rolList.Where(a => a.aplicacion.Codigo.Trim().ToLower() == aplicacion.Trim().ToLower()).ToList();


            if (!dPageInfo.Reverse)
                rolList = rolList.OrderBy(x => x.nombre).ToList();
            else
                rolList = rolList.OrderByDescending(x => x.nombre).ToList();

            rolResp.Total = rolList.Count();
            rolResp.Lista = rolList.Skip((dPageInfo.Page - 1) * dPageInfo.ItemsPerPage).Take(dPageInfo.ItemsPerPage).ToList();

            // Write the list to the response body.
           // HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, rolResp);
            return Ok(rolResp);
        }



        /// <summary>
        /// Recupera todos los permisos de un rol
        /// </summary>
        /// <param name="rolId"></param>
        /// <returns></returns>

        [HttpGet]        
        [Route("RolPermission")]
        public List<Permissions> RolPermission(int rolId)
        {
            return _rolService.GetRolePermission(rolId);

        }

        /// <summary>
        /// Recupera todos los permisos de un rol por aplicacion
        /// </summary>
        /// <param name="rolId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
     
        [HttpGet]
        [Route("RolPermissionByApp")]
        public List<Permissions> RolPermissionByApp(int rolId, int appId)
        {
            return _rolService.GetRolePermissionByApp(rolId, appId);

        }

    }
} 
