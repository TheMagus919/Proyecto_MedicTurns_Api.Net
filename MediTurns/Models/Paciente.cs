using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediTurns.Models{
    [Table("Pacientes")]
public class Paciente{
    [Key]
    public int IdPaciente { get; set;}

    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string Nombre { get; set;}

    [Required(ErrorMessage = "El campo Apellido es obligatorio")]
    public string Apellido { get; set;}

    [Required(ErrorMessage = "El campo Email es obligatorio")]
    [EmailAddress(ErrorMessage = "Email no válido")]
    public string Email { get; set;}

    [Required(ErrorMessage = "El campo DNI es obligatorio")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos")]
    public string Dni { get; set;}

    [Required(ErrorMessage = "El campo Cuil es obligatorio")]
    public string Cuil { get; set;}

    [Required(ErrorMessage = "El campo Telefono es obligatorio")]
    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "El teléfono debe tener entre 7 y 15 dígitos")]
    public string Telefono { get; set;}

    [Required]
    public string ObraSocial { get; set;}

    [Required(ErrorMessage = "El campo Direccion es obligatorio")]
    public string Direccion { get; set;}

    [Required(ErrorMessage = "El campo Grupo Sanguineo es obligatorio")]
    public string GrupoSanguineo { get; set;}

    [Required(ErrorMessage = "El campo Alergias es obligatorio")]
    public string Alergias { get; set;}

    [Required]
    public int IdRiesgo { get; set;}

    [ForeignKey(nameof(IdRiesgo))]
    public Riesgo? riesgo { get; set;}

}
}