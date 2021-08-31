using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("Rol", Schema = "Sistema")]
    public partial class Rol
    {
        public Rol()
        {
            RolPermiso = new HashSet<RolPermiso>();
            UsuarioRol = new HashSet<UsuarioRol>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaAlta { get; set; }
        public int AppId { get; set; }
        [StringLength(150)]
        public string Observaciones { get; set; }

        [ForeignKey(nameof(AppId))]
        [InverseProperty(nameof(Aplicacion.Rol))]
        public virtual Aplicacion App { get; set; }
        [InverseProperty("Rol")]
        public virtual ICollection<RolPermiso> RolPermiso { get; set; }
        [InverseProperty("Rol")]
        public virtual ICollection<UsuarioRol> UsuarioRol { get; set; }
    }
}
