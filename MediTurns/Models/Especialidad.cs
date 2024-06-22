using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediTurns.Models{
    [Table("Especialidades")]
public class Especialidad{
    [Key]
    public int IdEspecialidades { get; set;}

    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string Nombre { get; set;}

    public ICollection<Estudio> estudio { get; set; }
}
}