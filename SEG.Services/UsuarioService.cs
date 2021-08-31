/************************************************************************************************************
 * Descripción: Servicio Entidad usuario. 
 * Observaciones: 
 * Creado por: Gabriel Recio.
 * Historial de Revisiones: 
 * ----------------------------------------------------------------------------------------------- 
 * Fecha        Evento / Método Autor   Descripción 
 * ----------------------------------------------------------------------------------------------- 
 * 04/10/2016 19:02			Implementación inicial. 
 * ----------------------------------------------------------------------------------------------- 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using SEG.Repositories.DataContext;
using SEG.Repositories.Common;
using log4net;
using SEG.LocalViewModels;
using SEG.Mappers;
using GlobalView;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using SEG.Services.Helpers;
using Microsoft.Extensions.Options;
using SEG.Repositories;

namespace SEG.Services
{

    public interface IUsuarioService
    {
        UsuarioViewModel GetById(int id, string codApp);
        List<UsuarioViewModel> GetAll(string query, string codApp);
        List<UsuarioViewModel> GetByRol(string rolId);
        List<UsuarioViewModel> GetByRol(string rolId, string query, string codApp);
        int Insert(UsuarioViewModel viewModel);
        int Update(UsuarioViewModel viewModel);
        int cambiarPass(UsuarioViewModel viewModel);
        bool Delete(int id);
        UsuarioViewModel FindUser(string userName, string password);
        IEnumerable<string> GetUserPermission(int userId, string codeApp);

        List<PermisosCentroCostoViewModel> GetPermisosCentroCosto(int userId, string codeApp);
        UsuarioViewModel checkUser(string userName, string password, string codApp);
        UsuarioViewModel FindUserForaneo(string codApp, string userName);
        bool checkUserApp(int userId, string codApp);
        List<UsuarioViewModel> GetAllById(string[] ids);
        List<ForaneaExtViewModel> GetTipoNotaPedidoPermisoByUser(int userId, string codeApp);
        //List<DepartamentoPermisosViewModel> GetPermisosDepartamento(int userId, string codeApp, List<ForaneaExtViewModel> listaDepartamentos);
        List<DepartamentoPermisosViewModel> GetPermisosDepartamento(int userId, string codeApp);
    }
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioRolRepository _usuarioRolRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IOperacionesRepository _operacionesRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IAplicacionRepository _aplicacionRepository;
        private readonly ICentroCostoRepository _centroCostoRepository;
        // protected readonly ILog log = LogManager.GetLogger("ErrorLog","");
        private readonly AppSettings _appSettings;
        public UsuarioService(IUsuarioRepository usuarioRepository, IAplicacionRepository aplicacionRepository, IRolRepository rolRepository, IOptions<AppSettings> appSettings, IOperacionesRepository operacionesRepository, IUsuarioRolRepository usuarioRolRepository, IMenuRepository menuRepository, ICentroCostoRepository centroCostoRepository)
        {
            this._usuarioRepository = usuarioRepository;
            this._rolRepository = rolRepository;
            this._aplicacionRepository = aplicacionRepository;
            this._operacionesRepository = operacionesRepository;
            this._usuarioRolRepository = usuarioRolRepository;
            this._menuRepository = menuRepository;
            this._centroCostoRepository = centroCostoRepository;
            _appSettings = appSettings.Value;
            //log4net.Config.XmlConfigurator.ConfigureAndWatch();

        }
        public UsuarioViewModel GetById(int id, string codApp)
        {
            // var model = _usuarioRepository.GetById(id);
            var model = _usuarioRepository.FindByCondition(owner => owner.Id.Equals(id)).FirstOrDefault();

            var viewModelUsuario = UsuarioMapper.Mapper(model);
            var allRoles = new List<RolViewModel>();

            var roles = new List<RolViewModel>();

            var appId = _aplicacionRepository.Get().Where(p => p.Codigo.ToLower().Trim() == codApp.Trim().ToLower()).Select(p => p.Id).FirstOrDefault();

            if (codApp.ToLower() != "todas")
            {
                allRoles = _rolRepository.Get().Where(p => p.AppId == appId).Select(p => RolViewModel.Mapper(p)).ToList();
                roles = (from ur in _usuarioRolRepository.Get().Where(p => p.UsuarioId == model.Id)
                         join r in _rolRepository.Get().Where(p => p.AppId == appId).ToList()
                         on ur.RolId equals r.Id
                         select new RolViewModel() { id = r.Id, nombre = r.Nombre, activo = r.Activo, aplicacion = new AplicacionViewModel() { Id = r.App.Id, Codigo = r.App.Codigo, Descripcion = r.App.Descripcion } }).ToList();
            }
            else
            {
                allRoles = _rolRepository.Get().Select(p => RolViewModel.Mapper(p)).ToList();
                roles = (from ur in _usuarioRolRepository.Get().Where(p => p.UsuarioId == model.Id)
                         join r in _rolRepository.Get()
                         on ur.RolId equals r.Id
                         select new RolViewModel() { id = r.Id, nombre = r.Nombre, activo = r.Activo, aplicacion = new AplicacionViewModel() { Id = r.App.Id, Codigo = r.App.Codigo, Descripcion = r.App.Descripcion } }).ToList();
            }
            viewModelUsuario.roles = allRoles;
            if (roles != null && roles.Count >0 )
            {
                viewModelUsuario.rol = roles.Select(p => ForaneaViewModel.Mapper(p.id, p.nombre)).FirstOrDefault();
                foreach (var item in roles)
                {
                    var rol = viewModelUsuario.roles.Where(p => p.id == item.id).FirstOrDefault();
                    if (rol != null)
                    {
                        rol.asociado = true;
                    }
                }
            }

            //var SingletonCentroCosto = ListaCentroCostoSingleton.Instance.GetLista();
            //var centroCosto = SingletonCentroCosto.Where(p=>p.Id == model.CentroCostoId).FirstOrDefault();
            //viewModelUsuario.centroCosto = ForaneaExtViewModel.Mapper(centroCosto.Id,centroCosto.Codigo,centroCosto.Nombre);
            viewModelUsuario.centroCosto = ForaneaExtViewModel.Mapper(model.CentroCostoId, "", "");
            return viewModelUsuario;
        }



        public List<UsuarioViewModel> GetAll(string query, string codApp)
        {
            var viewModels = new List<UsuarioViewModel>();
            List<Usuario> usuarios;
            if (String.IsNullOrEmpty(query))
            {
                usuarios = (List<Usuario>)_usuarioRepository.Get();
            }
            else
            {
                usuarios = _usuarioRepository.Get().Where(p => p.Username.ToLower().Contains(query.ToLower()) || p.Nombre.ToLower().Contains(query.ToLower()) || p.Apellido.ToLower().Contains(query.ToLower())).ToList();
            }

            List<Rol> listaRoles = null;
            if (codApp.ToLower() != "todas")
            {
                var appId = _aplicacionRepository.Get().Where(p => p.Codigo.ToLower().Trim() == codApp.Trim().ToLower()).Select(p => p.Id).FirstOrDefault();

                usuarios = (from u in usuarios
                            join ur in _usuarioRolRepository.Get()
                            on u.Id equals ur.UsuarioId
                            join r in _rolRepository.Get().Where(p => p.AppId == appId).ToList()
                            on ur.RolId equals r.Id
                            select u).ToList();
            }
            var SingletonCentroCosto = _centroCostoRepository.GetAll(); //ListaCentroCostoSingleton.Instance.GetLista();            
            foreach (Usuario model in usuarios)
            {
                viewModels.Add(UsuarioMapper.Mapper(model, SingletonCentroCosto.Select(p=> CentroCostoMapper.Mapper(p)).ToList()));
            }
            return viewModels;
        }

        public List<UsuarioViewModel> GetByRol(string rolId,string query, string codApp)
        {
            var viewModels = new List<UsuarioViewModel>();
            List<Usuario> usuarios;
            if (String.IsNullOrEmpty(query))
            {
               
                usuarios = _usuarioRepository.GetusuarioByRol(rolId);
               
            }
            else
            {
                usuarios = _usuarioRepository.GetusuarioByRol(rolId).Where(p => p.Username.ToLower().Contains(query.ToLower()) || p.Nombre.ToLower().Contains(query.ToLower()) || p.Apellido.ToLower().Contains(query.ToLower())).ToList();
            }


            var SingletonCentroCosto = _centroCostoRepository.GetAll(); //ListaCentroCostoSingleton.Instance.GetLista();
            foreach (Usuario model in usuarios)
            {
                viewModels.Add(UsuarioMapper.Mapper(model, SingletonCentroCosto.Select(p => CentroCostoMapper.Mapper(p)).ToList()));
            }
            return viewModels;
        }
        public List<UsuarioViewModel> GetByRol(string rolId)
        {
            var viewModels = new List<UsuarioViewModel>();
            List<Usuario> usuarios;
            usuarios = _usuarioRepository.GetusuarioByRol(rolId);
            var SingletonCentroCosto = _centroCostoRepository.GetAll();//ListaCentroCostoSingleton.Instance.GetLista();
            foreach (Usuario model in usuarios)
            {
                viewModels.Add(UsuarioMapper.Mapper(model, SingletonCentroCosto.Select(p => CentroCostoMapper.Mapper(p)).ToList()));
            }
            return viewModels;
        }

        public List<UsuarioViewModel> GetAllById(string[] ids)
        {
            var viewModels = new List<UsuarioViewModel>();
            viewModels = _usuarioRepository.Get().Where(p => ids.Contains(p.Id.ToString())).Select(p => UsuarioMapper.Mapper(p)).ToList();
            return viewModels;
        }

        public int Insert(UsuarioViewModel viewModel)
        {
            try
            {
                var model = UsuarioMapper.Mapper(viewModel);
                model.Activo = true;
                if (!String.IsNullOrEmpty(viewModel.passwd))
                {
                    var h = new Hasher { SaltSize = 16 };
                    model.Password = h.Encrypt(viewModel.passwd);
                }
                if (viewModel.roles != null)
                {
                    foreach (var itemRol in viewModel.roles.Where(p => p.asociado == true))
                    {
                        var usuarioRol = new UsuarioRol()
                        {
                            RolId = itemRol.id,
                            UsuarioId = model.Id,
                            Activo = true,
                            FechaAlta = DateTime.Now
                        };
                        model.UsuarioRol.Add(usuarioRol);
                    }
                }

                _usuarioRepository.Create(model);
                _usuarioRepository.DbContext.SaveChanges();

                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public int Update(UsuarioViewModel viewModel)
        {
            try
            {

                var model = UsuarioMapper.Mapper(viewModel);
                _usuarioRepository.UsuarioUpdate(model);
                return model.Id;

            }
            catch (Exception e)
            {
                return 0;
            }
        }


        public int cambiarPass(UsuarioViewModel viewModel)
        {
            try
            {

                // var model = _usuarioRepository.GetById(viewModel.id);
                var model = _usuarioRepository.FindByCondition(owner => owner.Id.Equals(viewModel.id)).FirstOrDefault();
                model.CambiarPass = false;
                if (!String.IsNullOrEmpty(viewModel.passwd) && viewModel.passwd != "********")
                {
                    if (!String.IsNullOrEmpty(viewModel.passwd))
                    {
                        var h = new Hasher { SaltSize = 16 };
                        model.Password = h.Encrypt(viewModel.passwd);
                    }
                }
                _usuarioRepository.Update(model);
                _usuarioRepository.DbContext.SaveChanges();
                return model.Id;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                _usuarioRepository.Delete(id);
                _usuarioRepository.DbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public UsuarioViewModel FindUser(string userName, string password)
        {
            UsuarioViewModel user = new UsuarioViewModel();

            return user;
        }

        public IEnumerable<string> GetUserPermission(int userId, string codeApp)
        {
            var permission = new List<string>();
            var listaPer = _usuarioRepository.GetUserPermission(userId, codeApp, "Menu");
            permission.AddRange(listaPer.Select(item => item.Key + "." + item.Value));
            return permission;
            //return _usuarioRepository.GetUserPermission(userId, codeApp);
        }
        public List<PermisosCentroCostoViewModel> GetPermisosCentroCosto(int userId, string codeApp)
        {
            var ListaCentros = new List<PermisosCentroCostoViewModel>();
            try
            {
                //var model = _usuarioRepository.GetById(id);
                var listaPer = _usuarioRepository.GetUserPermission(userId, codeApp, "CentroCosto");

                foreach (var centro in listaPer.Select(p => p.Key).Distinct().ToList())
                { //verificar si existe el registro del centro de costo, debe existir una solo reg por centro costo.
                    var listaPermiso = new List<ForaneaViewModel>();
                    var SingletonCentroCosto = _centroCostoRepository.GetAll();  //ListaCentroCostoSingleton.Instance.GetLista();
                    var centroCosto = SingletonCentroCosto.Where(p => p.Nombre.ToLower() == centro.ToLower() && p.Activo == true).FirstOrDefault();
                    if(centroCosto != null) { 
                        foreach (var perm in listaPer.Where(p => p.Key == centro).ToList())
                        {
                            var item = new ForaneaViewModel()
                            {
                                id = 0,
                                descripcion = perm.Value
                            };
                            listaPermiso.Add(item);
                        }
                        ListaCentros.Add(new PermisosCentroCostoViewModel() { id = centroCosto.Id, codigo = centroCosto.Codigo, descripcion = centroCosto.Nombre, descripcionCompleta = centroCosto.Codigo + " " + centroCosto.Nombre, Permisos = listaPermiso });
                    }
                }

                return ListaCentros;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public List<DepartamentoPermisosViewModel> GetPermisosDepartamento(int userId, string codeApp)
        {
            var ListaCentros = new List<DepartamentoPermisosViewModel>();
            try
            {

                var listaPer = _usuarioRepository.GetUserPermission(userId, codeApp, "Departamento");
                
                var appId = _aplicacionRepository.Get().Where(p => p.Codigo.Trim().ToLower() == codeApp.Trim().ToLower()).Select(p => p.Id).FirstOrDefault();
                var entidadDepartamento = _aplicacionRepository.DbContext.EntidadTipo.Where(p => p.Nombre.Trim().ToLower() == "departamento").Select(p => p.Id).FirstOrDefault();
                
                foreach (var permiso in listaPer.Select(p => p.Key).Distinct().ToList())
                {
                    //verificar si existe el registro del depto, debe existir una solo reg por depto
                    var listaPermiso = new List<ForaneaViewModel>();

                    //var departamento = listaDepartamentos.Where(p => p.descripcion.ToLower() == permiso.ToLower()).FirstOrDefault();
                   

                    var departamento = _menuRepository.GetAll().Where(p => p.AppId == appId && p.TipoEntidadId == entidadDepartamento && p.Titulo.ToLower() == permiso.ToLower()).FirstOrDefault();
                    if(departamento!= null)
                    { 
                        foreach (var perm in listaPer.Where(p => p.Key == permiso).ToList())
                            {
                                var item = new ForaneaViewModel()
                                {
                                    id = 0,
                                    descripcion = perm.Value
                                };
                                listaPermiso.Add(item);
                            }
                         ListaCentros.Add(new DepartamentoPermisosViewModel() { id = Convert.ToInt32(departamento.Clave), codigo = departamento.Clave, descripcion = departamento.Titulo, descripcionCompleta = departamento.Clave + " " + departamento.Titulo, Permisos = listaPermiso });
                        // ListaCentros.Add(new DepartamentoPermisosViewModel() { id = departamento.id, codigo = departamento.codigo, descripcion = departamento.descripcion, descripcionCompleta = departamento.codigo + " " + departamento.descripcion, Permisos = listaPermiso });
                    }
                }

                return ListaCentros;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public List<ForaneaExtViewModel> GetTipoNotaPedidoPermisoByUser(int userId, string codeApp)
        {

            var listaPer = _usuarioRepository.GetUserPermission(userId, codeApp, "TipoNotaPedido");
            return listaPer.Select(p => ForaneaExtViewModel.Mapper(1, p.Key, p.Value)).ToList();
        }



        public UsuarioViewModel checkUser(string userName, string password, string codApp)
        {
            try
            {
                var h = new Hasher { SaltSize = 16 };
                var usuario = _usuarioRepository.Get().Where(u => u.Username.ToLower() == userName.ToLower() && u.Activo).FirstOrDefault();
                if (usuario == null)
                    return null;

                if (!h.CompareStringToHash(password, usuario.Password))
                    return null;

               var user= GetById(usuario.Id, codApp);
                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, usuario.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);

                return user.WithoutPassword();


            }
            catch (Exception e)
            {
              //  log.Info("usuario checkuser:" + e.InnerException.Message);
                return null;
            }
        }

        public bool checkUserApp(int userId, string codApp)
        {
            try
            {
                //var _dbContext = new SeguridadEntities();
                //var h = new Hasher { SaltSize = 16 };
                var app =  _aplicacionRepository.Get().Where(u => u.Codigo.Trim().ToLower() == codApp.Trim().ToLower()).FirstOrDefault();
                var usuario = _usuarioRepository.Get().Where(u => u.Id == userId && u.Activo && u.UsuarioRol.Any(x => x.Rol.AppId == app.Id)).FirstOrDefault();
                if (usuario != null)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public UsuarioViewModel FindUserForaneo(string codApp, string userName)
        {
            try
            {
                var user = _usuarioRepository.GetUsuarioForaneo(codApp, userName);
                if (user != null)
                {
                    return UsuarioMapper.Mapper(user);
                }
                else
                {
                    return null;
                }



            }
            catch (Exception e)
            {
              //  log.Info("Error FindUserForaneo codApp:" + codApp + " userName:" + userName + ". " + e.InnerException.Message);
                return null;
            }
        }




    }
}
