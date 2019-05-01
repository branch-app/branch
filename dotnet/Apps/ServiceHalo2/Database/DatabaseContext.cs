using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Branch.Apps.ServiceHalo2.Database
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext() { }

		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

		public DbSet<GamertagReplacement> GamertagReplacements { get; set; }

		public DbSet<ServiceRecordCache> ServiceRecordCaches { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<GamertagReplacement>().ToTable(nameof(GamertagReplacements));
			builder.Entity<ServiceRecordCache>().ToTable(nameof(ServiceRecordCaches));
		}
	}

	public class ServiceRecordCache
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string Id { get; set; }

		public bool Done { get; set; }

		public string Gamertag { get; set; }
	}

	public class GamertagReplacement
	{
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public string Id { get; set; }

		public string ReplacedGamertag { get; set; }

		public int ReplacingXUID { get; set; }
	}
}
