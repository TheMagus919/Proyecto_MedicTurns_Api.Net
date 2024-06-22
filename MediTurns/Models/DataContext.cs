using Microsoft.EntityFrameworkCore;
using MediTurns.Controllers;
using MediTurns.Models;

namespace WebApi.Models{
    public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		public DbSet<Horario> Horarios { get; set; }
		public DbSet<Usuario> Usuarios { get; set; }
		public DbSet<Paciente> Pacientes { get; set; }
		public DbSet<Dia> Dias { get; set; }
		public DbSet<Rol> Roles { get; set; }
        public DbSet<Riesgo> Riesgos { get; set; }
        public DbSet<Estudio> Estudios { get; set; }
        public DbSet<Especialidad> Especialidades { get; set; }
        public DbSet<Turno> Turnos { get; set; }
	}
}