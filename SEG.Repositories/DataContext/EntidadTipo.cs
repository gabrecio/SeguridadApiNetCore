using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("EntidadTipo", Schema = "Sistema")]
    public partial class EntidadTipo
    {
        public EntidadTipo()
        {
            Menu = new HashSet<Menu>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public bool Activo { get; set; }

        [InverseProperty("TipoEntidad")]
        public virtual ICollection<Menu> Menu { get; set; }
    }
}
