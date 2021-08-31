/************************************************************************************************************
 * Descripción: Controlador Entidad usuario. 
 * Observaciones: 
 * Creado por: Gabriel Recio.
 * Historial de Revisiones: 
 * ----------------------------------------------------------------------------------------------- 
 * Fecha        Evento / Método Autor   Descripción 
 * ----------------------------------------------------------------------------------------------- 
 * 04/10/2016 19:02		Implementación inicial. 
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SEG.Services;
using Newtonsoft.Json;

namespace SEG.API.Controllers
{
    [Authorize]
    [ApiController]
    /// <summary>
    /// Controlador de Usuarios
    /// </summary>
    [Route("api/[controller]")]
    public class UsuarioController : BaseController
    {
        private IUsuarioService _usuarioService = null;
        private IRolService _rolService = null;
        private ITangoSueldoService _tangoSueldoService = null;

        public UsuarioController(IUsuarioService usuarioService, IRolService rolService, ITangoSueldoService tangoSueldoService)
        {
            this._usuarioService = usuarioService;
            this._rolService = rolService;
            this._tangoSueldoService = tangoSueldoService;
        }
        // 
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           // log.Info("GET api/ usuario/" + id);
            //HttpResponseMessage response;
            var registro = _usuarioService.GetById(id, "todas");
            if (registro == null)
            {
               // log.Error("ERROR GET api /  usuario:  Status 404");
                //response = Request.CreateResponse(HttpStatusCode.NotFound, 0);
                return NotFound(0);
            }
            //registro.Roles = _rolService.GetRolesByUser(id);
            //response = Request.CreateResponse(HttpStatusCode.OK, registro);
            return Ok(registro);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetByApp")]
        public IActionResult GetByApp(int id, string codeApp)
        {
          //  log.Info("GET api/ usuario/" + id);
           // HttpResponseMessage response;
            var registro = _usuarioService.GetById(id, codeApp);
            if (registro == null)
            {
             //   log.Error("ERROR GET api /  usuario:  Status 404");
                //response = Request.CreateResponse(HttpStatusCode.NotFound, 0);
                return NotFound(0);
            }
            //registro.Roles = _rolService.GetRolesByUser(id);
            //response = Request.CreateResponse(HttpStatusCode.OK, registro);
            return Ok(registro);
        }

        [AllowAnonymous]
        [HttpGet]
        public List<UsuarioViewModel> Get()
        {
            //log.Info("GET api/usuario");
            List<UsuarioViewModel> lista = _usuarioService.GetAll("", "SEGUR").ToList();
            return lista;
        }


       
        [HttpPost("{id}")]
        public IActionResult Post(UsuarioViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                //log.Error("ERROR POST api / usuario: Model Invalid");
                return BadRequest(ModelState);
            }
           // log.Info("POST api/ usuario * id:" + viewModel.id);
            var resultado = _usuarioService.Insert(viewModel);
            if (resultado > 0)
                return Ok(new GenericResponse { Status = 200, Message = "El registro fue guardado correctamente." });
            else
                return Ok(new GenericResponse { Status = 203, Message = "Error al intentar guardar el registro." });

        }

        
        [HttpPut("{id}")]
        public IActionResult Put(UsuarioViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
               // log.Error("ERROR PUT api / usuario: Model Invalid");
                return BadRequest(ModelState);
            }
            try
            {
                _usuarioService.Update(viewModel);
            }
            catch (DbUpdateConcurrencyException e)
            {
              //  log.Error("ERROR PUT api/usuario: " + e.Message);
                return NotFound();
            }
            return Ok(new GenericResponse { Status = 200, Message = "El registro fue actualizado satisfactoriamente." });
        }

        /// <summary>
        /// Eliminar Usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
           // log.Info("DELETE api/ usuario * id: " + id);
            try
            {
                if (_usuarioService.Delete(id))
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
        /// Buscar usuarios segun nombre de usuario y retorna un listado paginado
        /// </summary>
        /// <param name="dPageInfo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("Search")]
        // public HttpResponseMessage Search(int itemsPerPage, int page, bool reverse, string search, string sortBy, int totalItems)
        public IActionResult Search([FromQuery] PageInfo dPageInfo)
        {

           // log.Info("GET /api/usuario/Search");
            var userResp = new GenericListResponse();
            List<UsuarioViewModel> userList;
        
            
                userList = _usuarioService.GetAll(dPageInfo.Search, dPageInfo.App).ToList();          
            

            userResp.Total = userList.Count();
            userResp.Lista = userList.Skip((dPageInfo.Page - 1) * dPageInfo.ItemsPerPage).Take(dPageInfo.ItemsPerPage).ToList();

            // Write the list to the response body.
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, userResp);
            return Ok(userResp);
        }


        /// <summary>
        /// Buscar usuarios segun nombre de usuario y retorna un listado paginado
        /// </summary>
        /// <param name="dPageInfo"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("SearchByRol")]
        // public HttpResponseMessage Search(int itemsPerPage, int page, bool reverse, string search, string sortBy, int totalItems)
        public IActionResult SearchByRol(int itemsPerPage, int page,string search,string aplication, string rolId)
        {

            //log.Info("GET /api/usuario/Search");
            var userResp = new GenericListResponse();
            List<UsuarioViewModel> userList;           
            userList = _usuarioService.GetByRol(rolId, search, aplication).ToList();
            userResp.Total = userList.Count;
            userResp.Lista = userList.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

            // Write the list to the response body.
            //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, userResp);
            return Ok(userResp);
        }
        /// <summary>
        /// Controla si el usuario tiene permiso en la aplicación
        /// </summary>
        /// <param name="userName">Nombre de Usuario</param>
        /// <param name="password">Contraseña</param>
        /// <param name="codeApp">Código de Aplicación</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("CheckUser")]
        public IActionResult CheckUser(string userName, string password, string codeApp)
        {

           // log.Info("GET /api/usuario/CheckUser");
           // HttpResponseMessage response;
            //hacer el check de usuario y pass
            var user = _usuarioService.checkUser(userName, password, codeApp);

            if (user != null)
            {
                if (!_usuarioService.checkUserApp(user.id, codeApp))
                {
                 //   log.Error("ERROR GET api /  usuario:  Status 401" + "El usuario no tiene permiso en la aplicación.");
                   // response = Request.CreateResponse(HttpStatusCode.Unauthorized, 0);
                    return Unauthorized(0);
                }
                //response = Request.CreateResponse(HttpStatusCode.OK, user);
                return Ok(user);
            }
            else
            {
                //log.Error("ERROR GET api /  usuario:  Status 401:" + "El usuario o la contraseña no son validos.");
                //response = Request.CreateResponse(HttpStatusCode.Unauthorized, 0);
                return Unauthorized(0);
            }

        }

        /// <summary>
        /// Recupera la lista de permisos para un usuari en una apicación
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="codeApp"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetUserPermission")]
        public IEnumerable<string> GetUserPermission(int userId, string codeApp)
        {

            try
            {

                var listaPermiso = _usuarioService.GetUserPermission(userId, codeApp);
                if (listaPermiso != null)
                    return listaPermiso;
                else return null;
            }
            catch (Exception e)
            {
                return null;

            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetCentoCostoPermisoByUser")]
        public List<PermisosCentroCostoViewModel> GetCentoCostoPermisoByUser(int userId)
        {
            try
            {
                var listaPermiso = _usuarioService.GetPermisosCentroCosto(userId, "NTPED");
                if (listaPermiso != null)
                    return listaPermiso;
                else return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("GetCentoCostoPresupuestoPermisoByUser")]
        public List<PermisosCentroCostoViewModel> GetCentoCostoPresupuestoPermisoByUser(int userId)
        {
            try
            {
                var listaPermiso = _usuarioService.GetPermisosCentroCosto(userId, "PRESU");
                if (listaPermiso != null)
                    return listaPermiso;
                else return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetDepartamentoPermisoByUser")]
        public List<DepartamentoPermisosViewModel> GetDepartamentoPermisoByUser(int userId)
        {
            try
            {
                //  var listaDepartamentos = _tangoSueldoService.GetListaDepartamentos("", "", 1, 300);
                //  var listaPermiso = _usuarioService.GetPermisosDepartamento(userId, "CTRH", listaDepartamentos);
                var listaPermiso = _usuarioService.GetPermisosDepartamento(userId, "CTRH");
                if (listaPermiso != null)
                    return listaPermiso;
                else return null;
            }
            catch (Exception e)
            {
                return null;

            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetTipoNotaPedidoPermisoByUser")]
        public List<ForaneaExtViewModel> GetTipoNotaPedidoPermisoByUser(int userId)
        {
            try
            {

                var listaPermiso = _usuarioService.GetTipoNotaPedidoPermisoByUser(userId, "NTPED");
                if (listaPermiso != null)
                    return listaPermiso;
                else return null;
            }
            catch (Exception e)
            {
                return null;

            }
        }


        /// <summary>
        /// Recupera una lista de usuarios segun su id
        /// </summary>
        /// <param name="usuarios"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("GetlistaDeUsuariosById")]
        public List<UsuarioViewModel> GetlistaDeUsuariosById(string usuarios)
        {

            //log.Info("GET /api/usuario/GetlistaDeUsuariosById");
            var ids = usuarios.Split(',');
            List<UsuarioViewModel> userList;

            try
            {
                userList = _usuarioService.GetAllById(ids).ToList();
                if (userList != null)
                    return userList;
                else return null;
            }
            catch (Exception e)
            {
                return null;

            }

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ValidaPass")]
        public bool ValidaPass(string userName, string pass)
        {
            //log.Info("POST api/ usuario * id:" + userName);
            var user = _usuarioService.checkUser(userName, pass, "");
            if (user != null)
                return true;
            else
                return false;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("CambiarPass")]
        public string CambiarPass(UsuarioViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                //log.Error("ERROR POST api / usuario: Model Invalid");
                return "0";
            }
            //log.Info("POST api/ usuario * id:" + viewModel.id);
            return _usuarioService.cambiarPass(viewModel).ToString();

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllByApp")]
        public List<UsuarioViewModel> GetAllByApp(string app)
        {
            //log.Info("GET api/GetAllByApp");
            List<UsuarioViewModel> lista = _usuarioService.GetAll("", app).ToList();
            return lista;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("GetUserForaneo")]
        public UsuarioViewModel GetUserForaneo(string codSistema, string usuario)
        {
            //log.Info("Get api/ GetUserForaneo codSistema:" + codSistema + " usuario:" + usuario);
            var user = _usuarioService.FindUserForaneo(codSistema, usuario);
            return user;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Token")]
        public UsuarioLoginViewModel Token([FromBody]AuthenticateModel auth)
        {
            var user = _usuarioService.checkUser(auth.username, auth.password, auth.codigoApp);
            if (user == null)
            {
                return null;
            }

            var listPermission = JsonConvert.SerializeObject(_usuarioService.GetUserPermission(user.id, auth.codigoApp));
            if (listPermission != null && listPermission.Any())
            {
                var usuario = new UsuarioLoginViewModel
                {
                    userId = user.id,
                    userName = user.username,
                    access_token = user.Token,
                    centroCosto = user.centroCosto.descripcion,
                    centroCostoId = user.centroCosto.id,
                    rol = user.rol.descripcion,
                    permissionList = listPermission
                };
               
                return usuario;
            }
            return null;
        }
    }
}
