using System;

namespace Branch.Models.Sql
{
	public class Audit
	{
		public Audit()
		{
			CreatedAt = DateTime.UtcNow;
			UpdatedAt = DateTime.UtcNow;
		}

		public DateTime CreatedAt { get; set; }

		public DateTime UpdatedAt { get; set; }
	}

}
