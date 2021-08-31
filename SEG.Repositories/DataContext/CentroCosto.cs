using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("CentroCosto", Schema = "Sistema")]
    public partial class CentroCosto
    {
        public CentroCosto()
        {
            InverseCentroCostoPadre = new HashSet<CentroCosto>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(5)]
        public string Codigo { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        public int? CentroCostoPadreId { get; set; }
        public bool Activo { get; set; }
        public bool ManejaPresupuesto { get; set; }

        [ForeignKey(nameof(CentroCostoPadreId))]
        [InverseProperty(nameof(CentroCosto.InverseCentroCostoPadre))]
        public virtual CentroCosto CentroCostoPadre { get; set; }
        [InverseProperty(nameof(CentroCosto.CentroCostoPadre))]
        public virtual ICollection<CentroCosto> InverseCentroCostoPadre { get; set; }
    }
}
