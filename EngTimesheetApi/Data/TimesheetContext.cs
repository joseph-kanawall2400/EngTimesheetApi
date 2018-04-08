using EngTimesheetApi.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EngTimesheetApi.Data
{
	public class TimesheetContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Token> Tokens { get; set; }
		public DbSet<Login> Logins { get; set; }
		public DbSet<Time> Times { get; set; }

		public TimesheetContext(DbContextOptions options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().ToTable(nameof(Users));
			modelBuilder.Entity<Token>().ToTable(nameof(Tokens));
			modelBuilder.Entity<Login>().ToTable(nameof(Logins));
			modelBuilder.Entity<Time>().ToTable(nameof(Times));
		}
	}
}
