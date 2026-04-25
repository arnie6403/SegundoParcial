using Microsoft.AspNetCore.Mvc;
using SegundoParcial.Data;
using SegundoParcial.Modelos;
using SegundoParcial.Modelos;

namespace SegundoParcial.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly string[] medicosValidos =
        {
            "MED-1010", "MED-2020", "MED-3030", "MED-4040", "MED-5050"
        };

        public PacientesController(AppDbContext context)
        {
            _context = context;
        }

        // ============================
        // POST
        // ============================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult CrearPaciente([FromBody] Paciente paciente)
        {
            // Validar médico
            if (!medicosValidos.Contains(paciente.MedicoResponsable))
                return Unauthorized("Médico no autorizado");

            // Validar estado
            var estadosValidos = new[] { "En espera", "Atendido", "Derivado" };
            if (!estadosValidos.Contains(paciente.Estado))
                return BadRequest("Estado inválido");

            // Validar capacidad crítica
            if (paciente.NivelGravedad == 5)
            {
                int count = _context.Pacientes
                    .Count(p => p.NivelGravedad == 5 && p.Estado == "En espera");

                if (count >= 5)
                    return BadRequest("Capacidad máxima alcanzada. Redirección inmediata a otro hospital sugerida");
            }

            // Generar ID
            int correlativo = _context.Pacientes.Count() + 1;
            paciente.IdPaciente = $"PAC-2026-{correlativo.ToString("D3")}";
            paciente.FechaIngreso = DateTime.UtcNow;

            _context.Pacientes.Add(paciente);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.IdPaciente }, paciente);
        }

        // ============================
        // GET ALL (ordenado manual)
        // ============================
        [HttpGet]
        public IActionResult GetPacientes()
        {
            var lista = _context.Pacientes.ToList();

            // Ordenamiento por inserción
            for (int i = 1; i < lista.Count; i++)
            {
                var actual = lista[i];
                int j = i - 1;

                while (j >= 0 &&
                      (lista[j].NivelGravedad < actual.NivelGravedad ||
                      (lista[j].NivelGravedad == actual.NivelGravedad &&
                       lista[j].FechaIngreso > actual.FechaIngreso)))
                {
                    lista[j + 1] = lista[j];
                    j--;
                }

                lista[j + 1] = actual;
            }

            return Ok(lista);
        }

        // ============================
        // GET BY ID
        // ============================
        [HttpGet("{id}")]
        public IActionResult GetPaciente(string id)
        {
            var paciente = _context.Pacientes.Find(id);

            if (paciente == null)
                return NotFound();

            return Ok(paciente);
        }
    }
}