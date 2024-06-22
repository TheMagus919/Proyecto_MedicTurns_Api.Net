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
    public class RiesgosController : Controller
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public RiesgosController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
		}

        // GET: api/Riesgo
        [HttpGet]
        public async Task<ActionResult<Riesgo>> Get()
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                if(Rol==1){
                    var listaRiesgos = await contexto.Riesgos.ToListAsync();
                    return Ok(listaRiesgos);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // POST: api/Riesgo/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Riesgo>> Post([FromBody] Riesgo riesgo)
		{
			try
			{   
                Console.WriteLine(riesgo);
                if(riesgo != null){
                    contexto.Riesgos.Add(riesgo);
                    await contexto.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = riesgo.IdRiesgo }, riesgo);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // PUT: api/Riesgo/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Riesgo>> Put(int id, [FromBody] Riesgo riesgo)
		{
			try
			{   
                if(riesgo != null){
                    contexto.Riesgos.Update(riesgo);
                    await contexto.SaveChangesAsync();
                    return Ok(riesgo);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // DELETE api/Riesgo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                contexto.Riesgos.Remove(new Riesgo { IdRiesgo = id });
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
