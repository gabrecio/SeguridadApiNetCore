using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SEG.Repositories.DataContext
{
    [Table("Usuario", Schema = "Sistema")]
    public partial class Usuario
    {
        public Usuario()
        {
            UsuarioForaneo = new HashSet<UsuarioForaneo>();
            UsuarioRol = new HashSet<UsuarioRol>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }
        [Required]
        [StringLength(50)]
        public string Mail { get; set; }
        public bool Activo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaAlta { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [StringLength(45)]
        public string Username { get; set; }
        public int CentroCostoId { get; set; }
        public bool CambiarPass { get; set; }
        [StringLength(10)]
        public string Legajo { get; set; }
        [StringLength(20)]
        public string Documento { get; set; }
        [StringLength(20)]
        public string Interno { get; set; }
        [StringLength(1)]
        public string Sexo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FechaNacimiento { get; set; }

        [InverseProperty("Usuario")]
        public virtual ICollection<UsuarioForaneo> UsuarioForaneo { get; set; }
        [InverseProperty("Usuario")]
        public virtual ICollection<UsuarioRol> UsuarioRol { get; set; }
    }
}
