using System.ComponentModel.DataAnnotations;

namespace Tp_AppVet.Models
{
    public class FichaMedica
    {
        public int Id { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Ingrese una Observación")]
        public string Observaciones { get; set; }
        [Required(ErrorMessage = "Seleccione una Mascota")]
        public int MascotaId { get; set; }
        public Mascota Mascota { get; set; }
    }
}
