using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediTurns.Models{
    [Table("Dias")]
public class Dia{
    [Key]
    public int IdDia { get; set;}

    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string Nombre { get; set;}

}
}
