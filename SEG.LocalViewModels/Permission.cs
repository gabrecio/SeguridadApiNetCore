using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEG.LocalViewModels
{
    public class Permissions
    {
        public string Menu { get; set; }
        public string Imagen { get; set; }
        public bool EsCentroCosto { get; set; }
        public List<Permission> Operaciones { get; set; }
    }

    public class Permission
    {
        public string Operacion { get; set; }
        public bool Activo { get; set; }
        public string Imagen { get; set; }
        public int ListaPermisoId { get; set; }
    }
}