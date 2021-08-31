using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("UsuarioForaneo", Schema = "Sistema")]
    public partial class UsuarioForaneo
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int SistemaForaneoId { get; set; }
        [Required]
        [StringLength(50)]
        public string CodUsuarioForaneo { get; set; }

        [ForeignKey(nameof(SistemaForaneoId))]
        [InverseProperty("UsuarioForaneo")]
        public virtual SistemaForaneo SistemaForaneo { get; set; }
        [ForeignKey(nameof(UsuarioId))]
        [InverseProperty("UsuarioForaneo")]
        public virtual Usuario Usuario { get; set; }
    }
}
