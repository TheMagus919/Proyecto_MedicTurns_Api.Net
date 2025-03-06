using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediTurns.Models{
    [Table("Turnos")]
public class Turno{
    [Key]
    public int IdTurno { get; set;}

    [Required]
    public int IdUsuario { get; set;}

    [Required]
    public int IdPaciente { get; set;}

    [Required]
    public int IdEstudio { get; set;}

    [Required(ErrorMessage = "El campo Fecha Turno es obligatorio")]
    public DateTime? FechaTurno { get; set;}

    [Required(ErrorMessage = "El campo Fecha Fin es obligatorio")]
    public DateTime? FechaFin { get; set;}

    [Required]
    public bool Asistio { get; set;}

    [Required]
    public string observaciones { get; set;}

    [ForeignKey(nameof(IdPaciente))]
    public Paciente? paciente { get; set;}

    [ForeignKey(nameof(IdEstudio))]
    public Estudio? estudio { get; set; }

    [ForeignKey(nameof(IdUsuario))]
    public Usuario? usuario { get; set; }

}
}