using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("ListaPermiso", Schema = "Sistema")]
    public partial class ListaPermiso
    {
        public ListaPermiso()
        {
            RolPermiso = new HashSet<RolPermiso>();
        }

        [Key]
        public int Id { get; set; }
        public int OperacionId { get; set; }
        public int MenuId { get; set; }

        [ForeignKey(nameof(MenuId))]
        [InverseProperty("ListaPermiso")]
        public virtual Menu Menu { get; set; }
        [ForeignKey(nameof(OperacionId))]
        [InverseProperty("ListaPermiso")]
        public virtual Operacion Operacion { get; set; }
        [InverseProperty("ListaPermiso")]
        public virtual ICollection<RolPermiso> RolPermiso { get; set; }
    }
}
