using System.ComponentModel.DataAnnotations;
namespace Tp_AppVet.Models
{
    public class Mascota
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Ingrese un Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese la especie")]
        public string Especie {  get; set; }
        [Required(ErrorMessage = "Ingrese la raza")]
        public string Raza { get; set; }
        [Required(ErrorMessage = "Ingrese la fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        [Required(ErrorMessage = "Seleccione un cliente")]
        public int ClienteId { get; set; }
        [Required]
        public Cliente Cliente { get; set; }
        [Required]
        public FichaMedica FichaMedica { get; set; }
    }
}
