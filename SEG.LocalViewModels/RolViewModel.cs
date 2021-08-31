/************************************************************************************************************
 * Descripción: ViewModel Contrato Entidad rol. 
 * Observaciones: 
 * Creado por: Gabriel Recio.
 * Historial de Revisiones: 
 * ----------------------------------------------------------------------------------------------- 
 * Fecha        Evento / Método Autor   Descripción 
 * ----------------------------------------------------------------------------------------------- 
 * 04/10/2016 18:24			Implementación inicial. 
 * ----------------------------------------------------------------------------------------------- 
*/
using SEG.Repositories.DataContext;
using System;
using System.Collections.Generic;

namespace SEG.LocalViewModels
{ 
    public class RolViewModel
    { 
        #region Propiedades 
        public int id { get; set; }
        public string nombre { get; set; }
        public bool activo { get; set; }
        public string observaciones { get; set; }
        public bool asociado { get; set; }
        public DateTime? fechaAlta { get; set; }
        public AplicacionViewModel aplicacion { get; set; }
        public List<Permissions> permisos { get; set; }
        #endregion Propiedades  - Get/Set


        public static RolViewModel Mapper(Rol model) 
        {
            return new RolViewModel()
            {
                id = model.Id,
                activo = model.Activo,
                asociado = false,
                nombre = model.Nombre,
                fechaAlta = model.FechaAlta, 
                observaciones = model.Observaciones,
                aplicacion = new AplicacionViewModel { Id= model.App.Id, Codigo = model.App.Codigo , Descripcion = model.App.Descripcion, Activo = model.App.Activo, Observaciones = model.App.Observaciones}
            };
        }

        public static Rol Mapper(RolViewModel viewModel)
        {
            return new Rol()
            {
                Id = viewModel.id,
                Activo = viewModel.activo,
                Nombre = viewModel.nombre, 
                AppId = viewModel.aplicacion.Id,
                Observaciones = viewModel.observaciones           
            };
        }
    } 
 
} 
