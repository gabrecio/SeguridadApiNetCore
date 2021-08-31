using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("Operacion", Schema = "Sistema")]
    public partial class Operacion
    {
        public Operacion()
        {
            ListaPermiso = new HashSet<ListaPermiso>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(60)]
        public string Nombre { get; set; }
        [StringLength(100)]
        public string Imagen { get; set; }
        public short Orden { get; set; }

        [InverseProperty("Operacion")]
        public virtual ICollection<ListaPermiso> ListaPermiso { get; set; }
    }
}
