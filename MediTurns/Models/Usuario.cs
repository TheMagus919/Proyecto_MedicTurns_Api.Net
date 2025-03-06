using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MediTurns.Models{
    [Table("Usuarios")]
public class Usuario{
    [Key]
    public int IdUsuario { get; set;}

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

    [Required(ErrorMessage = "El campo Clave es obligatorio")]
    public string Clave { get; set;}

    [Required(ErrorMessage = "El campo Telefono es obligatorio")]
    [RegularExpression(@"^\d{7,15}$", ErrorMessage = "El teléfono debe tener entre 7 y 15 dígitos")]
    public string Telefono { get; set;}

    [Required]
    public int IdRol { get; set;}

    [AllowNull]
    public int? IdEspecialidades { get; set;}

    [ForeignKey(nameof(IdEspecialidades))]
    public Especialidad? especialidad { get; set;}

    [ForeignKey(nameof(IdRol))]
    public Rol? rol { get; set;}

}
}