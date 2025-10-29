using System.ComponentModel.DataAnnotations;

namespace Tp_AppVet.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Ingrese su Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Elija un rol")]
        public string Rol { get; set; } = "Cliente";
    }
}
