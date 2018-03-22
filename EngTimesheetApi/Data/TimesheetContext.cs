using EngTimesheetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EngTimesheetApi.Data
{
	public class TimesheetContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Token> Tokens { get; set; }
		public DbSet<Login> Logins { get; set; }

		public TimesheetContext(DbContextOptions options) : base(options) { }
	}
}
