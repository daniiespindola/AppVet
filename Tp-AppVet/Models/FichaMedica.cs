using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tp_AppVet.Models
{
    public class FichaMedica
    {
        [Key]
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Ingrese una Observación")]
        public string Observaciones { get; set; }
        [Required(ErrorMessage = "Seleccione una Mascota")]

        // 🔹 Relación 1 a 1 con Mascota
        [ForeignKey("Mascota")]
        public int MascotaId { get; set; }
        public Mascota Mascota { get; set; }
    }
}
