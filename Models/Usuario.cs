using System.ComponentModel.DataAnnotations;

namespace WebApplication6.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }

        [StringLength(50)]
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Usu { get; set; }

        [StringLength(50)]
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contrasena { get; set; }

        [StringLength(100)]
        [Display(Name = "Confirmar Contraseña")]
        [Required(ErrorMessage = "La confirmación de contraseña es requerida")]
        public string CContrasena { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "El correo es requerido")]
        public string Correo { get; set; }

        [StringLength(50)]
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "El teléfono es requerido")]
        public string telefono { get; set; }

        [StringLength(100)]
        [Display(Name = "Dirección")]
        [Required(ErrorMessage = "La dirección es requerida")]
        public string Direccion { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateTime FechaNacimiento { get; set; }
    }
}
