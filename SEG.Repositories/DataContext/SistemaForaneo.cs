using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("SistemaForaneo", Schema = "Sistema")]
    public partial class SistemaForaneo
    {
        public SistemaForaneo()
        {
            UsuarioForaneo = new HashSet<UsuarioForaneo>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Sistema { get; set; }
        [StringLength(500)]
        public string Observaciones { get; set; }

        [InverseProperty("SistemaForaneo")]
        public virtual ICollection<UsuarioForaneo> UsuarioForaneo { get; set; }
    }
}
