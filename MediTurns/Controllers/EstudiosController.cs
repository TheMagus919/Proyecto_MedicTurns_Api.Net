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
    public class EstudiosController : Controller
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public EstudiosController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
		}

        // GET: api/Estudio
        [HttpGet]
        public async Task<ActionResult<Estudio>> Get()
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                if(Rol==1){
                    var listaEstudios = await contexto.Estudios.ToListAsync();
                    return Ok(listaEstudios);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // POST: api/Estudio/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Estudio>> Post([FromBody] Estudio estudio)
		{
			try
			{   
                Console.WriteLine(estudio);
                if(estudio != null){
                    contexto.Estudios.Add(estudio);
                    await contexto.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = estudio.IdEstudio }, estudio);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // PUT: api/Estudio/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Estudio>> Put(int id, [FromBody] Estudio estudio)
		{
			try
			{   
                if(estudio != null){
                    contexto.Estudios.Update(estudio);
                    await contexto.SaveChangesAsync();
                    return Ok(estudio);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // DELETE api/Estudio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                contexto.Estudios.Remove(new Estudio { IdEstudio = id });
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
