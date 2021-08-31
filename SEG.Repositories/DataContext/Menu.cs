using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("Menu", Schema = "Sistema")]
    public partial class Menu
    {
        public Menu()
        {
            ListaPermiso = new HashSet<ListaPermiso>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Clave { get; set; }
        [StringLength(200)]
        public string Imagen { get; set; }
        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }
        [StringLength(200)]
        public string OrderBy { get; set; }
        public int AppId { get; set; }
        public int? TipoEntidadId { get; set; }

        [ForeignKey(nameof(AppId))]
        [InverseProperty(nameof(Aplicacion.Menu))]
        public virtual Aplicacion App { get; set; }
        [ForeignKey(nameof(TipoEntidadId))]
        [InverseProperty(nameof(EntidadTipo.Menu))]
        public virtual EntidadTipo TipoEntidad { get; set; }
        [InverseProperty("Menu")]
        public virtual ICollection<ListaPermiso> ListaPermiso { get; set; }
    }
}
