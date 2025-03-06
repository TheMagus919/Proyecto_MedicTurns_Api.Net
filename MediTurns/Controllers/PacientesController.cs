using MediTurns.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace MediTurns.Controllers
{   
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PacientesController : Controller
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public PacientesController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
		}

        // GET: /Pacientes/42278146
        [HttpGet("{Dni}")]
        public async Task<ActionResult<Paciente>> Get(string Dni)
		{
			try
			{   
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                if(Rol==1 || Rol==2){
                    var paciente = await contexto.Pacientes.Include(p=>p.riesgo).SingleOrDefaultAsync(p=>p.Dni==Dni);
                    if (paciente == null)
                    {
                        return NotFound($"Paciente con DNI {Dni} no encontrado");
                    }
                    return Ok(paciente);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
        [HttpPost("crear")]
        public async Task<ActionResult<Paciente>> Post(
            [FromForm(Name = "Nombre")] string nombre,
            [FromForm(Name = "Apellido")] string apellido,
            [FromForm(Name = "Dni")] string dni,
            [FromForm(Name = "Cuil")] string cuil,
            [FromForm(Name = "Email")] string email,
            [FromForm(Name = "Telefono")] string telefono,
            [FromForm(Name = "ObraSocial")] string obraSocial,
            [FromForm(Name = "Direccion")] string direccion,
            [FromForm(Name = "GrupoSanguineo")] string grupoSanguineo,
            [FromForm(Name = "Alergias")] string alergias,
            [FromForm(Name = "IdRiesgo")] int idRiesgo)
        {
            try
            {
                var paciente = new Paciente
                {
                    Nombre = nombre,
                    Apellido = apellido,
                    Dni = dni,
                    Cuil = cuil,
                    Email = email,
                    Telefono = telefono,
                    ObraSocial = obraSocial,
                    Direccion = direccion,
                    GrupoSanguineo = grupoSanguineo,
                    Alergias = alergias,
                    IdRiesgo = idRiesgo
                };

                if (paciente != null)
                {
                    contexto.Pacientes.Add(paciente);
                    await contexto.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = paciente.IdPaciente }, paciente);
                }
                else
                {
                    return BadRequest("No es posible dejar campos vacíos");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message} | StackTrace: {ex.StackTrace}");
            }
        }

        // PUT: /Pacientes/5
        [HttpPut("editar/{id}")]
        public async Task<ActionResult<Paciente>> Put([FromRoute]int id, 
            [FromForm(Name = "Nombre")] string nombre,
            [FromForm(Name = "Apellido")] string apellido,
            [FromForm(Name = "Email")] string email,
            [FromForm(Name = "Dni")] string dni,
            [FromForm(Name = "Cuil")] string cuil,
            [FromForm(Name = "Telefono")] string telefono,
            [FromForm(Name = "ObraSocial")] string obraSocial,
            [FromForm(Name = "Direccion")] string direccion,
            [FromForm(Name = "GrupoSanguineo")] string grupoSanguineo,
            [FromForm(Name = "Alergias")] string alergias,
            [FromForm(Name = "IdRiesgo")] int idRiesgo)
		{
			try
			{   
                var paciente = new Paciente
                {   
                    IdPaciente = id,
                    Nombre = nombre,
                    Apellido = apellido,
                    Email = email,
                    Dni = dni,
                    Cuil = cuil,
                    Telefono = telefono,
                    ObraSocial = obraSocial,
                    Direccion = direccion,
                    GrupoSanguineo = grupoSanguineo,
                    Alergias = alergias,
                    IdRiesgo = idRiesgo
                };
                if(paciente != null){
                    contexto.Pacientes.Update(paciente);
                    await contexto.SaveChangesAsync();
                    return Ok(paciente);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // GET: /Pacientes
        [HttpGet]
        public async Task<ActionResult<Paciente>> Get()
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                if(Rol==1){
                    var listaPacientes = await contexto.Pacientes.ToListAsync();
                    return Ok(listaPacientes);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // DELETE /Pacientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                contexto.Pacientes.Remove(new Paciente { IdPaciente = id });
                await contexto.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
