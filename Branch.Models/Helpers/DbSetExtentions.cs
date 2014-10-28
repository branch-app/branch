using System.Data.Entity;

namespace Branch.Models.Helpers
{
	public static class DbSetExtentions
	{
		public static void Clear<T>(this DbSet<T> dbSet)
			where T : class
		{
			dbSet.RemoveRange(dbSet);
		}
	}
}
