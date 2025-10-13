using System.ComponentModel.DataAnnotations;

namespace Tp_AppVet.Models
{
    public class Turno
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Ingrese una fecha")]
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "Ingrese una hora")]
        public string Hora { get; set; }
        [Required(ErrorMessage = "Ingrese el motivo")]
        public string Motivo { get; set; }
        [Required(ErrorMessage ="Ingrese el id del dueño")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        [Required(ErrorMessage ="Ingrese una mascota")]
        public int MascotaId { get; set; }
        public Mascota Mascota { get; set; }
        [Required]
        public int VeterinarioId { get; set; }
        public Veterinario Veterinario { get; set; }
    }
}
