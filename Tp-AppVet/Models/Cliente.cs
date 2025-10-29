using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tp_AppVet.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre no puede estar vacío")]
        public  string Nombre { get; set; }
        [Required(ErrorMessage = "El apellido no puede estar vacío")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "El DNI no puede estar vacío")]
        public string Dni {  get; set; }
        
        [Required(ErrorMessage = "El telefono no puede estar vacío")]
        public string Telefono { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public ICollection<Mascota>? Mascotas { get; set; }
        public ICollection<Turno>? Turnos { get; set; }
    }
}
