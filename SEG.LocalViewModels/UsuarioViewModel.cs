/************************************************************************************************************
 * Descripción: ViewModel Contrato Entidad usuario. 
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
using SEG.Repositories.DataContext;
using System.Linq;
using GlobalView;

namespace SEG.LocalViewModels
{
   public class UsuarioViewModel
    {
        #region Propiedades 
        public int id { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string mail { get; set; }
        public bool activo { get; set; }
        public DateTime? fechaAlta { get; set; }
        public string passwd { get; set; }
        public string username { get; set; }
        public ForaneaViewModel rol { get; set; }
        public ForaneaExtViewModel centroCosto { get; set; }
        public List<RolViewModel> roles { get; set; }
        public bool cambiarPass { get; set; }
        public string legajo { get; set; }
        public string interno { get; set; }
        public string documento { get; set; }
        public string Token { get; set; }

        #endregion Propiedades  - Get/Set

    }

    public class UsuarioLoginViewModel
    {
        #region Propiedades 
        public int userId { get; set; }
        public string centroCosto { get; set; }
        public int centroCostoId { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }      
        public string userName { get; set; }
        public string rol { get; set; }              
        public string legajo { get; set; }
      
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string permissionList { get; set; }

        #endregion Propiedades  - Get/Set

    }
} 
