using System.Data.Entity;

namespace Branch.Models.Sql
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext()
			: base("DefaultConnection") { }

		public DbSet<Authentication> Authentications { get; set; }
	}
}
