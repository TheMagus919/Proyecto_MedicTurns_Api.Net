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
        [HttpGet("{dni}")]
        public async Task<ActionResult<Paciente>> Get(int dni)
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                if(Rol==1 || Rol==2){
                    var paciente = await contexto.Pacientes.Include(p=>p.riesgo).SingleOrDefaultAsync(p=>p.Dni==dni.ToString());
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

        // POST: /Pacientes/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Paciente>> Post([FromForm] Paciente paciente)
		{
			try
			{   
                Console.WriteLine(paciente);
                if(paciente != null){
                    contexto.Pacientes.Add(paciente);
                    await contexto.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = paciente.IdPaciente }, paciente);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
        
        // PUT: /Pacientes/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Paciente>> Put(int id, [FromForm] Paciente paciente)
		{
			try
			{   
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
