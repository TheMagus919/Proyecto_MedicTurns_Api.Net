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
    public class TurnosController : Controller
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public TurnosController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
		}

        // GET: /Turnos
        [HttpGet]
        public async Task<ActionResult<Turno>> Get()
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                int id = int.Parse(claimsList[1].Value);
                DateTime fechaHoy = DateTime.Now;
                if(Rol==2){
                    var listaTurnos = await contexto.Turnos
                                        .Include(t=>t.estudio)
                                            .ThenInclude(e=>e.especialidad)
                                        .Include(t=>t.estudio)
                                            .ThenInclude(e=>e.riesgo)
                                        .Include(t=>t.paciente)
                                            .ThenInclude(p=>p.riesgo)
                                        .Where(t=>t.IdUsuario==id && t.FechaTurno.Value.Date == fechaHoy.Date).ToListAsync();
                    return Ok(listaTurnos);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // GET: /Turnos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Turno>> Get(int id)
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                int idUser = int.Parse(claimsList[1].Value);
                if(Rol==1 || Rol==2){
                    var turno = await contexto.Turnos
                                .Include(t=>t.estudio)
                                    .ThenInclude(e=>e.especialidad)
                                .Include(t=>t.estudio)
                                    .ThenInclude(e=>e.riesgo)
                                .Include(t=>t.paciente)
                                    .ThenInclude(p=>p.riesgo)
                                .SingleOrDefaultAsync(x=>x.IdTurno==id);
                    return Ok(turno);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // GET: /Turnos/historial/42278146
        [HttpGet("historial/{dni}")]
        public async Task<ActionResult<Turno>> GetHistorial(int dni)
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                int idUser = int.Parse(claimsList[1].Value);
                if(Rol==1 || Rol==2){
                    var listaTurnos = contexto.Turnos
                                        .Include(t=>t.estudio)
                                            .ThenInclude(e=>e.especialidad)
                                        .Include(x=>x.estudio)
                                            .ThenInclude(e=>e.riesgo)
                                        .Include(t=>t.paciente)
                                            .ThenInclude(p=>p.riesgo)
                                        .Where(x => x.paciente.Dni==dni.ToString()).ToList();;
                    return Ok(listaTurnos);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        //PUT: /Turnos/editar/5
        [HttpPut("editar/{id}")]
        public async Task<ActionResult<Turno>> Editar(int id, [FromForm] Turno turno)
		{
			try
			{   
                if(turno != null){
                    contexto.Turnos.Update(turno);
                    await contexto.SaveChangesAsync();
                    return Ok(turno);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
        //PUT: /Turnos/observacion/5
        [HttpPut("observacion/{id}")]
        public async Task<ActionResult<Turno>> Observacion(int id, [FromForm] Turno turno)
		{
			try
			{   
                if(turno != null){
                    contexto.Turnos.Update(turno);
                    await contexto.SaveChangesAsync();
                    return Ok(turno);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
        // POST: /Turnos/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Turno>> Post([FromBody] Turno turno)
		{
			try
			{   
                Console.WriteLine(turno);
                if(turno != null){
                    contexto.Turnos.Add(turno);
                    await contexto.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = turno.IdTurno }, turno);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // PUT: /Turnos/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Turno>> Put(int id, [FromBody] Turno turno)
		{
			try
			{   
                if(turno != null){
                    contexto.Turnos.Update(turno);
                    await contexto.SaveChangesAsync();
                    return Ok(turno);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // DELETE /Turnos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                contexto.Turnos.Remove(new Turno { IdTurno = id });
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
