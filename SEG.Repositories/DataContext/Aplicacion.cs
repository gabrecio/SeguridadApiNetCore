using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("Aplicacion", Schema = "Sistema")]
    public partial class Aplicacion
    {
        public Aplicacion()
        {
            Menu = new HashSet<Menu>();
            Rol = new HashSet<Rol>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(5)]
        public string Codigo { get; set; }
        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        [StringLength(10)]
        public string Color { get; set; }
        [StringLength(150)]
        public string Observaciones { get; set; }

        [InverseProperty("App")]
        public virtual ICollection<Menu> Menu { get; set; }
        [InverseProperty("App")]
        public virtual ICollection<Rol> Rol { get; set; }
    }
}
