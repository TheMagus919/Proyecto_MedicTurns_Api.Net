using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediTurns.Models{
    [Table("Roles")]
public class Rol{
    [Key]
    public int IdRol { get; set;}

    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string Nombre { get; set;}

}
}