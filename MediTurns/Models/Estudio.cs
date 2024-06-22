using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediTurns.Models{
    [Table("Estudios")]
public class Estudio{
    [Key]
    public int IdEstudio { get; set;}

    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string Nombre { get; set;}

    [Required(ErrorMessage = "El campo Descripcion es obligatorio")]
    public string Descripcion { get; set;}

    [Required(ErrorMessage = "El campo Precio es obligatorio")]
    public double Precio { get; set;}

    [Required(ErrorMessage = "El campo Requisitos es obligatorio")]
    public string Requisitos { get; set;}

    [Required]
    public int IdRiesgo { get; set;}

    [ForeignKey(nameof(IdRiesgo))]
    public Riesgo? riesgo { get; set;}

    [Required]
    public int IdEspecialidades { get; set;}

    [ForeignKey(nameof(IdEspecialidades))]
    public Especialidad? especialidad { get; set;}
    public ICollection<Turno> Turno { get; set; }

}
}