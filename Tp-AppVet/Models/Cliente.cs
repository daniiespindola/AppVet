using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tp_AppVet.Models
{
    [Table("CLIENTES")]
    public class Cliente
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre no puede estar vacío")]
        public required string Nombre { get; set; }
        
        [Required(ErrorMessage = "El mail no puede estar vacío")]
        public required string Email { get; set; }
        
        [Required(ErrorMessage = "El telefono no puede estar vacío")]
     
        public required string Telefono { get; set; }
        

    }
}
