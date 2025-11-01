using System.ComponentModel.DataAnnotations;

namespace Tp_AppVet.Models
{
    public class Veterinario
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="Ingrese su nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese su Apellido")]
        public string Apellido { get; set; }
        [Required(ErrorMessage = "Ingrese una Matricula")]
        public string Matricula { get; set; }
        [Required(ErrorMessage = "Ingrese su Especialidad")]
        public string Especialidad { get; set; }
        [Required]
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public ICollection<Turno>? Turnos { get; set; }
    }
}
