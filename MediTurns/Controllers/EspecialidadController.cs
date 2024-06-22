using MediTurns.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace MediTurns.Controllers
{   
	[ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EspecialidadController : Controller
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public EspecialidadController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
		}

        // GET: api/Especialidad
        [HttpGet]
        public async Task<ActionResult<Especialidad>> Get()
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                if(Rol==1){
                    var listaEspecialidades = await contexto.Especialidades.ToListAsync();
                    return Ok(listaEspecialidades);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // POST: api/Especialidad/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Especialidad>> Post([FromBody] Especialidad especialidad)
		{
			try
			{   
                Console.WriteLine(especialidad);
                if(especialidad != null){
                    contexto.Especialidades.Add(especialidad);
                    await contexto.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = especialidad.IdEspecialidades }, especialidad);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // PUT: api/Especialidad/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Especialidad>> Put(int id, [FromBody] Especialidad especialidad)
		{
			try
			{   
                if(especialidad != null){
                    contexto.Especialidades.Update(especialidad);
                    await contexto.SaveChangesAsync();
                    return Ok(especialidad);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // DELETE api/Especialidad/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                contexto.Especialidades.Remove(new Especialidad { IdEspecialidades = id });
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
