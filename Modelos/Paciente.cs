using System.ComponentModel.DataAnnotations;

namespace SegundoParcial.Modelos
{
    public class Paciente
    {
        [Key]
        public string IdPaciente { get; set; }

        [Required]
        [Range(1, 5)]
        public int NivelGravedad { get; set; }

        [Required]
        public string Estado { get; set; }

        [Required]
        public string MedicoResponsable { get; set; }

        public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
    }
}