using Branch.Apps.ServiceHalo4.Models.Waypoint.Common;

namespace Branch.Apps.ServiceHalo4.Models.Waypoint.ServiceRecord
{
	public class Specialization
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public ImageUrl ImageUrl { get; set; }

		public int Level { get; set; }

		public string LevelName { get; set; }

		public double PercentComplete { get; set; }

		public bool IsCurrent { get; set; }

		public bool Completed { get; set; }
	}
}
