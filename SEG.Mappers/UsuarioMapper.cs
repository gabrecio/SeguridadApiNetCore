using GlobalView;
using SEG.LocalViewModels;
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEG.Mappers
{
    public class UsuarioMapper
    {

      
        public static UsuarioViewModel Mapper(Usuario model, List<CentroCostoViewModel> lCentroCosto)
        {

            //util a futuro ---   var listaAplicaciones = string.Join("-", model.UsuarioRols.Select(m => m.Rol.Aplicacion.Codigo).Distinct().ToArray());
            var centro = lCentroCosto.Where(p => p.Id == model.CentroCostoId).FirstOrDefault();
            var usuario = new UsuarioViewModel()
            {
                id = model.Id,
                username = model.Username,
                nombre = model.Nombre,
                apellido = model.Apellido,
                mail = model.Mail,
                fechaAlta = model.FechaAlta,
                activo = model.Activo,
                rol = null,
                roles = null,//new ForaneaViewModel() { id = rol.Id, descripcion = rol.Rol.Nombre },
                centroCosto = new ForaneaExtViewModel() { id = model.CentroCostoId, codigo = centro.Codigo, descripcion = centro.Nombre },
                cambiarPass = model.CambiarPass,
                legajo = model.Legajo,
                interno = model.Interno,
                documento = model.Documento

            };
            //usuario.listaCentroCosto = model.CentroCostoUsuarios.Select(p => new ForaneaExtViewModel() { id = p.CentroCosto.Id, codigo = p.CentroCosto.Codigo, descripcion = p.CentroCosto.Nombre }).ToList();
            return usuario;

        }

        public static UsuarioViewModel Mapper(Usuario model)
        {

            var usuario = new UsuarioViewModel()
            {
                id = model.Id,
                username = model.Username,
                nombre = model.Nombre,
                apellido = model.Apellido,
                mail = model.Mail,
                fechaAlta = model.FechaAlta,
                activo = model.Activo,
                roles = null,// new ForaneaViewModel() { id = 0, descripcion = "" },
                rol = null,
                centroCosto = null,
                //  listaCentroCosto = new List<ForaneaExtViewModel>(),
                cambiarPass = model.CambiarPass,
                legajo = model.Legajo,
                interno = model.Interno,
                documento = model.Documento
            };
            //usuario.listaCentroCosto = model.CentroCostoUsuarios.Select(p => new ForaneaExtViewModel() { id = p.CentroCosto.Id, codigo = p.CentroCosto.Codigo, descripcion = p.CentroCosto.Nombre }).ToList();
            return usuario;

        }

        public static Usuario Mapper(UsuarioViewModel viewModel)
        {
            var roles = new List<UsuarioRol>();
            if (viewModel.roles != null)
            {
                foreach (var itemRol in viewModel.roles.Where(p => p.asociado == true))
                {
                    var usuarioRol = new UsuarioRol()
                    {
                        RolId = itemRol.id,
                        UsuarioId = viewModel.id,
                        Activo = true,
                        FechaAlta = DateTime.Now
                    };
                    roles.Add(usuarioRol);
                }
            }
            var usuario = new Usuario()
            {
                Id = viewModel.id,
                Username = viewModel.username,
                Nombre = viewModel.nombre,
                Apellido = viewModel.apellido,
                Mail = viewModel.mail,
                FechaAlta = DateTime.Now,
                Password = viewModel.passwd,
                UsuarioRol = roles,
                CambiarPass = viewModel.cambiarPass,
                CentroCostoId = viewModel.centroCosto?.id ?? 0,
                Activo = viewModel.activo,
                Legajo = viewModel.legajo,
                Interno = viewModel.interno,
                Documento = viewModel.documento
            };
            return usuario;
        }
    }
}
