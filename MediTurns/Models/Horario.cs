using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediTurns.Models{
    [Table("Horarios")]
public class Horario{
    [Key]
    public int IdHorario { get; set;}

    [Required]
    public int IdDia { get; set;}

    [Required(ErrorMessage = "El campo Hora Inicio es obligatorio")]
    public DateTime? horaInicio { get; set;}

    [Required(ErrorMessage = "El campo Hora Fin es obligatorio")]
    public DateTime? horaFin { get; set;}

    [Required]
    public int IdUsuario { get; set;}

}
}