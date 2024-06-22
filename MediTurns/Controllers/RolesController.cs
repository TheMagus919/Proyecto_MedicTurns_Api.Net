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
    public class RolesController : Controller
    {
       private readonly DataContext contexto;
		private readonly IConfiguration config;
		private readonly IWebHostEnvironment environment;

		public RolesController(DataContext contexto, IConfiguration config, IWebHostEnvironment env)
		{
			this.contexto = contexto;
			this.config = config;
			environment = env;
		}

        // GET: api/Rol
        [HttpGet]
        public async Task<ActionResult<Rol>> Get()
		{
			try
			{
				var claimsList = User.Claims.ToList();
                int Rol = int.Parse(claimsList[2].Value);
                if(Rol==1){
                    var listaRoles = await contexto.Roles.ToListAsync();
                    return Ok(listaRoles);
                }else{
                    return BadRequest("No tienes permisos");
                }
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // POST: api/Rol/crear
        [HttpPost("crear")]
        public async Task<ActionResult<Rol>> Post([FromBody] Rol rol)
		{
			try
			{   
                Console.WriteLine(rol);
                if(rol != null){
                    contexto.Roles.Add(rol);
                    await contexto.SaveChangesAsync();
                    return CreatedAtAction(nameof(Get), new { id = rol.IdRol }, rol);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // PUT: api/Rol/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Rol>> Put(int id, [FromBody] Rol rol)
		{
			try
			{   
                if(rol != null){
                    contexto.Roles.Update(rol);
                    await contexto.SaveChangesAsync();
                    return Ok(rol);
                }else{
                    return BadRequest("No es posible dejar campos vacios");
                }

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // DELETE api/Rol/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                contexto.Roles.Remove(new Rol { IdRol = id });
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
