using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using MediTurns.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace MediTurns.Controllers
{	
	[Route("[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
    public class UsuariosController : Controller
    {   
		private readonly DataContext contexto;
        private readonly IConfiguration config;
        private readonly IWebHostEnvironment environment;
        private String[] roles = {"Secretaria", "Doctor"};
		public UsuariosController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
		{
			this.contexto = contexto;
			this.config = config;
			this.environment = environment;
		}

        [HttpPost("login")]
        [AllowAnonymous]
        // POST: Usuarios/Login/
        public async Task<IActionResult> Login([FromForm] LoginView loginView){
				try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
				var u = await contexto.Usuarios.FirstOrDefaultAsync(x => x.Email == loginView.Usuario);
				if (u == null || u.Clave != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				}
				else
				{
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, u.Email),
						new Claim("Id",u.IdUsuario+""),
						new Claim(ClaimTypes.Role, u.IdRol+""),
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(60),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

        // GET: Usuarios/perfil
		[HttpGet("perfil")]
        public async Task<ActionResult<Usuario>> Get()
        {	
			var usuario = User.Identity.Name;
			return await contexto.Usuarios.Include(u=>u.rol).SingleOrDefaultAsync(x => x.Email == usuario);
        }
		
		// GET: Usuarios/Logout
		[HttpGet("logout")]
		public async Task<ActionResult> Logout()
		{
			return Ok( new {message = "Sesion cerrada Exitosamente."});
		}

		[HttpGet("login")]
		[AllowAnonymous]
		// GET: Usuarios/Login/
		public ActionResult Login(string returnUrl)
		{
			TempData["Url"] = returnUrl;
			return View();
		}

		// POST: Usuarios/Crear
		[HttpPost("Crear")]
		[AllowAnonymous]
		public async Task<IActionResult> Post([FromBody] Usuario u)
		{
			try
			{	
				if(u!=null)
				{	
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
								password: u.Clave,
								salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
								prf: KeyDerivationPrf.HMACSHA1,
								iterationCount: 1000,
								numBytesRequested: 256 / 8));
					u.Clave= hashed;
					var nbreRnd = Guid.NewGuid();
					contexto.Usuarios.Add(u);
					await contexto.SaveChangesAsync();
					return Ok();
				}
				return BadRequest("No es posible dejar campos vacios");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

    }
}
