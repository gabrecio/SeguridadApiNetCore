using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("UsuarioRol", Schema = "Sistema")]
    public partial class UsuarioRol
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int RolId { get; set; }
        public bool Activo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaAlta { get; set; }

        [ForeignKey(nameof(RolId))]
        [InverseProperty("UsuarioRol")]
        public virtual Rol Rol { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        [InverseProperty("UsuarioRol")]
        public virtual Usuario Usuario { get; set; }
    }
}
