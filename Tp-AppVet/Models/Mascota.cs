using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Tp_AppVet.Models
{
    public class Mascota
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Ingrese un Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese la especie")]
        public string Especie {  get; set; }
        [Required(ErrorMessage = "Ingrese la raza")]
        public string Raza { get; set; }
        [Required(ErrorMessage = "Ingrese la Edad")]
        public int Edad { get; set; }

        // 🔹 Relación con Cliente (uno a muchos)
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // 🔹 Relación 1 a 1 con Ficha Médica
        public FichaMedica? FichaMedica { get; set; }

        // 🔹 Relación 1 a muchos con Turnos
        public ICollection<Turno>? Turnos { get; set; }
    }
}
