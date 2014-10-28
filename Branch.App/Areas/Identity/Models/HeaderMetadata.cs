namespace Branch.App.Areas.Identity.Models
{
	public class HeaderMetadata
	{
		public HeaderMetadata(string title, string description)
		{
			Title = title;
			Description = description;
		}

		public string Title { get; set; }

		public string Description { get; set; }
	}
}