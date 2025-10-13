using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tp_AppVet.Models
{
    public class Usuario
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage ="Ingrese su Email")]
        public string Email { get; set; }
        [Required(ErrorMessage ="Ingrese una contraseña")]
        public string PasswordHash {  get; set; }
        [Required(ErrorMessage ="Elija un rol")]
        public string Rol {  get; set; }
    }
}
