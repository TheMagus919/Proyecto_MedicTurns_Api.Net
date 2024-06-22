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
    public class DiasController : Controller
    {
        private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public DiasController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
		}

        // GET: api/Dias
        [HttpGet]
        public async Task<ActionResult<Dia>> Get()
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                if(Rol==1){
                    var listaDias = contexto.Dias.ToList();
                    return Ok(listaDias);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // POST: api/Dia/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Dia>> Post([FromBody] Dia dia)
		{
			try
			{   
                if(dia != null){
                    contexto.Dias.Add(dia);
                    await contexto.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = dia.IdDia }, dia);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // PUT: api/Dia/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Dia>> Put(int id, [FromBody] Dia dia)
		{
			try
			{   
                if(dia != null){
                    contexto.Dias.Update(dia);
                    await contexto.SaveChangesAsync();
                    return Ok(dia);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // DELETE api/Dia/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                contexto.Dias.Remove(new Dia { IdDia = id });
                await contexto.SaveChangesAsync();
                return Ok("Eliminado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
