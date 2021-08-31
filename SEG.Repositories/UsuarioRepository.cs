/************************************************************************************************************
 * Descripción: Repositorio Entidad usuario. 
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
using System.Linq;
using SEG.Repositories.Common;
using SEG.Repositories.DataContext;
using System.Collections.Generic;
using System.Data.Entity;

namespace SEG.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        // IEnumerable<string> GetUserPermission(int userId, string codeApp);
        List<KeyValuePair<string, string>> GetUserPermission(int userId, string codeApp, string entidad);
        int UsuarioUpdate(Usuario user);
        Usuario GetUsuarioForaneo(string app, string userName);
        List<Usuario> GetusuarioByRol(string rolId);
    }
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    { 
        private readonly SeguridadEntities _dbContext;
        public UsuarioRepository(SeguridadEntities dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

      
        public List<KeyValuePair<string, string>> GetUserPermission(int userId, string codeApp, string entidad)
        {            
            try
            {
                var listRoles = new List<Rol>();
                var app = _dbContext.Aplicacion.Where(p => p.Codigo.Trim().ToLower() == codeApp.Trim().ToLower()).FirstOrDefault();
                /*listRoles = (from ur in _dbContext.UsuarioRol
                           join r in _dbContext.Rol
                           on ur.RolId equals r.Id
                           where ur.UsuarioId == userId
                           && r.AppId == app.Id
                           select r).ToList();
                 var resultado = (from l in listRoles.ToList().SelectMany(p => p.RolPermiso).ToList()
                               join op in _dbContext.Operacion
                                     on l.ListaPermiso.OperacionId equals op.Id into ope
                               from x in ope
                               join menu in _dbContext.Menu.Where(p => p.TipoEntidad.Nombre.ToLower() == entidad.ToLower())
                                     on l.ListaPermiso.MenuId equals menu.Id into men
                               from subPerm in men                                    
                               select new KeyValuePair<string, string>((string)(subPerm == null ? String.Empty : subPerm.Titulo), (string)(x == null ? String.Empty : x.Nombre))).ToList();*/

                var listPermisos = (from ur in _dbContext.UsuarioRol
                             join r in _dbContext.Rol
                             on ur.RolId equals r.Id
                             join p in _dbContext.RolPermiso
                             on r.Id equals p.RolId
                             where ur.UsuarioId == userId
                             && r.AppId == app.Id
                             select p.ListaPermiso).ToList();

                var resultado = (from l in listPermisos
                                 join op in _dbContext.Operacion
                                       on l.OperacionId equals op.Id into ope
                                 from x in ope
                                 join menu in _dbContext.Menu.Where(p => p.TipoEntidad.Nombre.ToLower() == entidad.ToLower())
                                       on l.MenuId equals menu.Id into men
                                 from subPerm in men
                                 select new KeyValuePair<string, string>((string)(subPerm == null ? String.Empty : subPerm.Titulo), (string)(x == null ? String.Empty : x.Nombre))).Distinct().ToList();               
            
                return resultado;
            }
            catch (Exception e)
            {
                return null;


            }
        }


        public int UsuarioUpdate(Usuario user)
        {
            // var currentUser= this.GetById(user.Id);

            var currentUser = this.FindByCondition(owner => owner.Id.Equals(user.Id)).FirstOrDefault();

            currentUser.Activo = user.Activo;
            currentUser.Nombre = user.Nombre;
            currentUser.Apellido = user.Apellido;
            currentUser.Mail = user.Mail;
            currentUser.Username = user.Username;
            currentUser.CambiarPass = user.CambiarPass;
            currentUser.Documento = user.Documento;
            currentUser.Interno = user.Interno;
            currentUser.Legajo = user.Legajo;
            if (!String.IsNullOrEmpty(user.Password) && user.Password != "********")
            {
                if (!String.IsNullOrEmpty(user.Password))
                {
                    var h = new Hasher { SaltSize = 16 };
                    currentUser.Password = h.Encrypt(user.Password);
                }
            }

            var flagRol = false;
            foreach (var item in user.UsuarioRol.Where(x => !currentUser.UsuarioRol.Select(l => l.RolId).Contains(x.RolId)).ToList())
            {
                //inserta nuevos
                currentUser.UsuarioRol.Add(item);
                _dbContext.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                flagRol = true;
            }

            foreach (var item in currentUser.UsuarioRol.Where(x => !user.UsuarioRol.Select(l => l.RolId).Contains(x.RolId)).ToList())
            {
                //elimina items 
                currentUser.UsuarioRol.Remove(item);
                _dbContext.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                flagRol = true;
            }
            if (flagRol)
                _dbContext.SaveChanges();

            _dbContext.Usuario.Attach(currentUser);
            _dbContext.Entry(currentUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _dbContext.SaveChanges();


            return 1;
        }

        public Usuario GetUsuarioForaneo(string app, string userName)
        {
          
            int appint;
            if(Int32.TryParse(app,out appint))
            {
                if (_dbContext.UsuarioForaneo.Where(x => x.SistemaForaneoId == appint && x.CodUsuarioForaneo == userName.ToLower()).Count()>0)
                {
                    return _dbContext.UsuarioForaneo.Where(x => x.SistemaForaneoId == appint && x.CodUsuarioForaneo == userName.ToLower()).First().Usuario;
                }
                
                // return _dbContext.Usuarios.Where(x => x.Username == "procautomatico").First();
            }
            return null;



        }
        public List<Usuario> GetusuarioByRol(string rolId)
        {
            int rolint;
            if (Int32.TryParse(rolId, out rolint))
            {
                var listId = _dbContext.UsuarioRol.Where(x => x.RolId == rolint).Select(x=>x.UsuarioId).ToList();
                return _dbContext.Usuario.Where(x => listId.Contains(x.Id)).ToList(); 
            }
            return null;
        }
    }
}
