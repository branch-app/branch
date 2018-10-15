using Branch.Packages.Models.External.Halo4;

namespace Branch.Packages.Models.Halo4.ServiceRecord
{
	public class Specialization
	{
		public int ID { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public string ImageUrl { get; set; }

		public int Level { get; set; }

		public double Completion { get; set; }

		public bool Current { get; set; }

		public bool Complete { get; set; }
	}
}
