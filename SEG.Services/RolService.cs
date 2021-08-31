/************************************************************************************************************
 * Descripción: Servicio Entidad rol. 
 * Observaciones: 
 * Creado por: Gabriel Recio.
 * Historial de Revisiones: 
 * ----------------------------------------------------------------------------------------------- 
 * Fecha        Evento / Método Autor   Descripción 
 * ----------------------------------------------------------------------------------------------- 
 * 04/10/2016 18:24			Implementación inicial. 
 * ----------------------------------------------------------------------------------------------- 
*/ 
using System;
using System.Collections.Generic;
using System.Linq;
using SEG.LocalViewModels;
using SEG.Repositories.DataContext;
using SEG.Repositories;
using System.Data.Entity;

namespace SEG.Services
{
    public interface IRolService
    {
        RolViewModel GetById(int id);
        List<RolViewModel> GetAll(string query);
        int Insert(RolViewModel viewModel);
        int Update(RolViewModel viewModel);
        bool Delete(int id);
        List<RolViewModel> GetRolesByUser(int userId);
        List<Permissions> GetRolePermission(int rolId);
        List<Permissions> GetRolePermissionByApp(int rolId, int appId);
    }
    public class RolService:IRolService
    { 
        private readonly IRolRepository _rolRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IListaPermisoRepository _listaPermisoRepository;
        private readonly IOperacionesRepository _operacionesRepository;
        public RolService(IRolRepository  rolRepository, IUsuarioRepository usuarioRepository, IMenuRepository menuRepository, IListaPermisoRepository listaPermisoRepository, IOperacionesRepository operacionesRepository)
        { 
            this._rolRepository =  rolRepository;
            this._usuarioRepository = usuarioRepository;
            this._menuRepository = menuRepository;
            this._listaPermisoRepository = listaPermisoRepository;
            this._operacionesRepository = operacionesRepository;
        } 
    public  RolViewModel GetById(int id)
    { 
            var rol = _rolRepository.FindByCondition(owner => owner.Id.Equals(id)).FirstOrDefault();
            return RolViewModel.Mapper(rol);
            //return RolViewModel.Mapper(_rolRepository.GetById(id));
        } 
    public  List<RolViewModel> GetAll(string query)
    { 
        var viewModels = new List<RolViewModel>();
        List<Rol> models;
        if (String.IsNullOrEmpty(query))
        {
                // models =  _rolRepository.Get(includeProperties: "App").ToList();
                models = _rolRepository.Get().ToList();
            }
        else
        {
                // models = _rolRepository.Get(includeProperties: "App").Where(p=>p.Nombre.ToLower().Contains(query.ToLower()) || p.App.Descripcion.ToLower().Contains(query.ToLower())).ToList();
                models = _rolRepository.Get().Where(p => p.Nombre.ToLower().Contains(query.ToLower()) || p.App.Descripcion.ToLower().Contains(query.ToLower())).ToList();
            }
        foreach (Rol model in models)
        {
            viewModels.Add(RolViewModel.Mapper(model));
        }
        return viewModels;
    } 
    public int Insert(RolViewModel viewModel)
    {
        try
        {
            var model =   ViewModelToModel(viewModel);
            _rolRepository.Create(model);
            _rolRepository.DbContext.SaveChanges();
                return 1;
        }
        catch (Exception e)
        {
            return 0;
        }
    }
    public int Update(RolViewModel viewModel)
    {
        try
        {
              
                var model = ViewModelToModel(viewModel);               
                _rolRepository.RolUpdate(model);

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
            _rolRepository.Delete(id);
            _rolRepository.DbContext.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

        public List<RolViewModel> GetRolesByUser(int userId)
        {
            Usuario user = _usuarioRepository.Get().Where(p => p.Id == userId).FirstOrDefault();
            var roles = _rolRepository.Get(includeProperties: "App").Where(p => p.UsuarioRol.Any(x=>x.Usuario.Id == user.Id));
            var listaRoles = new List<RolViewModel>();
            foreach(Rol item in roles)
            {
                listaRoles.Add(RolViewModel.Mapper(item));
            }
            return listaRoles;

        }

        public List<Permissions> GetRolePermissionByApp(int rolId, int appId)
        {
            var rolPermissions = new List<Permissions>();
            try
            {

                var rolPermission = new List<int>();
                if (rolId != 0)
                {
                    rolPermission =
                        (from rol in _rolRepository.Get(includeProperties: "App").FirstOrDefault(i => i.Id == rolId).RolPermiso
                         select rol.ListaPermisoId).ToList();
                }

                foreach (var menu in _menuRepository.Get().Where(p=>p.AppId==appId))
                {
                    var perm = new Permissions()
                    {
                        Menu = menu.Titulo,
                        Imagen = menu.Imagen,
                        Operaciones = null
                    };
                    rolPermissions.Add(perm);
                }

                var menuAterior = "";
                var count = 0;
                var last =
                    (from lp in _listaPermisoRepository.Get().Where(p => p.Menu.AppId == appId)
                     join op in _operacionesRepository.Get() on lp.OperacionId equals op.Id
                     select lp).ToList()
                        .OrderBy(m => m.MenuId).Last();

                var listaOperaciones = new List<Permission>();

                foreach (var ope in _listaPermisoRepository.Get().Where(p=>p.Menu.AppId == appId).OrderBy(m => m.MenuId))
                {
                    if (count == 0)
                    {
                        menuAterior = ope.Menu.Titulo;
                        count++;
                    }


                    if (ope.Menu.Titulo != menuAterior)
                    {
                        var firstOrDefault = rolPermissions.FirstOrDefault(m => m.Menu.Equals(menuAterior));
                        if (firstOrDefault != null)
                            firstOrDefault.Operaciones = listaOperaciones;
                        listaOperaciones = new List<Permission>();
                        menuAterior = ope.Menu.Titulo;
                    }


                    var np = new Permission()
                    {
                        Operacion = ope.Operacion.Nombre,
                        Activo = rolPermission.Contains(ope.Id),
                        Imagen = ope.Operacion.Imagen,
                        ListaPermisoId = ope.Id
                    };
                    listaOperaciones.Add(np);

                    if (ope.Equals(last))
                    {
                        var firstOrDefault = rolPermissions.FirstOrDefault(m => m.Menu.Equals(menuAterior));
                        if (firstOrDefault != null)
                            firstOrDefault.Operaciones = listaOperaciones;
                    }


                }


            }
            catch (Exception excepcion)
            {
                return null;
            }
            return rolPermissions;

        }

        public List<Permissions> GetRolePermission(int rolId)
        {
            var rolPermissions = new List<Permissions>();
            try
            {

                var rolPermission = new List<int>();
                if (rolId != 0)
                {
                    rolPermission =
                        (from rol in _rolRepository.Get(includeProperties: "App").FirstOrDefault(i => i.Id == rolId).RolPermiso 
                         select rol.ListaPermisoId).ToList();
                }

                foreach (var menu in _menuRepository.Get())
                {
                    var perm = new Permissions()
                    {
                        Menu = menu.Titulo,
                        Imagen = menu.Imagen,
                        Operaciones = null
                    };
                    rolPermissions.Add(perm);
                }

                var menuAterior = "";
                var count = 0;
                var last =
                    (from lp in _listaPermisoRepository.Get()
                         join op in _operacionesRepository.Get() on lp.OperacionId equals op.Id  select lp).ToList()
                        .OrderBy(m => m.MenuId).Last();

                var listaOperaciones = new List<Permission>();
              
                foreach (var ope in _listaPermisoRepository.Get().OrderBy(m => m.MenuId))
                {
                    if (count == 0)
                    {
                        menuAterior = ope.Menu.Titulo;
                        count++;
                    }


                    if (ope.Menu.Titulo != menuAterior)
                    {
                        var firstOrDefault = rolPermissions.FirstOrDefault(m => m.Menu.Equals(menuAterior));
                        if (firstOrDefault != null)
                            firstOrDefault.Operaciones = listaOperaciones;
                        listaOperaciones = new List<Permission>();
                        menuAterior = ope.Menu.Titulo;
                    }


                    var np = new Permission()
                    {
                        Operacion = ope.Operacion.Nombre,
                        Activo = rolPermission.Contains(ope.Id),
                        Imagen = ope.Operacion.Imagen,
                        ListaPermisoId = ope.Id
                    };
                    listaOperaciones.Add(np);

                    if (ope.Equals(last))
                    {
                        var firstOrDefault = rolPermissions.FirstOrDefault(m => m.Menu.Equals(menuAterior));
                        if (firstOrDefault != null)
                            firstOrDefault.Operaciones = listaOperaciones;
                    }


                }


            }
            catch (Exception excepcion)
            {
                return null;
            }
            return rolPermissions;

        }

        #region private
  /*   private RolViewModel ModelToViewModel(sisRol model)
    {
        if (model != null)
        {
            var viewModel = new RolViewModel()
            {
                Id= model.id,
                Nombre= model.nombre,
                Activo= model.activo,
                FechaAlta= model.fechaAlta,
            };
            return viewModel;
        }
        return null;
    }*/

    private List<ListaPermiso> ViewListaPermisoToModel(List<Permissions> lista)
    {
            var listaPermiso = new List<ListaPermiso>();
            foreach (Permissions lip in lista)
            {
                if(lip.Operaciones != null) { 
                    foreach (Permission p in lip.Operaciones)
                    {
                        if (p.Activo)
                            //listaPermiso.Add(_listaPermisoRepository.GetById(p.ListaPermisoId));
                            listaPermiso.Add(_listaPermisoRepository.FindByCondition(owner => owner.Id.Equals(p.ListaPermisoId)).FirstOrDefault());
                    }
                }
            }
            return listaPermiso;
    }

        private List<RolPermiso> ViewListaPermisoToModel2(List<Permissions> lista, int rolId)
        {
            var listaPermiso = new List<RolPermiso>();
            foreach (Permissions lip in lista)
            {
                if (lip.Operaciones != null)
                {
                    foreach (Permission p in lip.Operaciones)
                    {
                        if (p.Activo)
                            //listaPermiso.Add(_listaPermisoRepository.GetById(p.ListaPermisoId).RolPermiso.FirstOrDefault());
                            listaPermiso.Add(_listaPermisoRepository.FindByCondition(owner => owner.Id.Equals(p.ListaPermisoId)).FirstOrDefault().RolPermiso.Select(p=> new RolPermiso { RolId = rolId, ListaPermisoId = p.ListaPermisoId }).FirstOrDefault());
                    }
                }
            }
            return listaPermiso;
        }
        private Rol ViewModelToModel( RolViewModel viewModel)
    {
        if (viewModel != null)
        {

            var model = new Rol()
            {
                Id= viewModel.id,
                Nombre= viewModel.nombre,
                Activo= viewModel.activo,
                FechaAlta= viewModel.fechaAlta ?? DateTime.Now,
                AppId = viewModel.aplicacion.Id,
                RolPermiso = ViewListaPermisoToModel2(viewModel.permisos, viewModel.id),
                Observaciones = viewModel.observaciones
                
            };

            return model;
        }
        return null;
    }
    #endregion
    }
}
