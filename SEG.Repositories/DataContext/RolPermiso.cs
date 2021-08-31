using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("RolPermiso", Schema = "Sistema")]
    public partial class RolPermiso
    {
        [Key]
        public int RolId { get; set; }
        [Key]
        public int ListaPermisoId { get; set; }

        [ForeignKey(nameof(ListaPermisoId))]
        [InverseProperty("RolPermiso")]
        public virtual ListaPermiso ListaPermiso { get; set; }
        [ForeignKey(nameof(RolId))]
        [InverseProperty("RolPermiso")]
        public virtual Rol Rol { get; set; }
    }
}
