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
    public class HorariosController : Controller
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public HorariosController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
		}

        // GET: api/Horario
        [HttpGet]
        public async Task<ActionResult<Horario>> Get()
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                if(Rol==1){
                    var listaHorarios = await contexto.Horarios.ToListAsync();
                    return Ok(listaHorarios);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // POST: api/Horario/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Horario>> Post([FromBody] Horario horario)
		{
			try
			{   
                Console.WriteLine(horario);
                if(horario != null){
                    contexto.Horarios.Add(horario);
                    await contexto.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = horario.IdHorario }, horario);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // PUT: api/Horario/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Horario>> Put(int id, [FromBody] Horario horario)
		{
			try
			{   
                if(horario != null){
                    contexto.Horarios.Update(horario);
                    await contexto.SaveChangesAsync();
                    return Ok(horario);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // DELETE api/Horario/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                contexto.Horarios.Remove(new Horario { IdHorario = id });
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
