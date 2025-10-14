using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tp_AppVet.Models
{
    public class Turno
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Ingrese una fecha")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Ingrese el motivo")]
        public string Motivo { get; set; }

        // 🔹 Relación con Cliente
        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // 🔹 Relación con Mascota
        [ForeignKey("Mascota")]
        public int MascotaId { get; set; }
        public Mascota Mascota { get; set; }

        // 🔹 Relación con Veterinario
        [ForeignKey("Veterinario")]
        public int VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; }
    }
}
