using System;
using Branch.Extenders;

namespace Branch.Models.Services.Branch
{
	public sealed class BlogPostEntity : BaseEntity
	{
		public static readonly string RowKeyString = "Post_{0}";
		public static readonly string PartitionKeyString = "Blog";

		public static string FormatRowKey(string ending)
		{
			return String.Format(RowKeyString, ending.ToTitleCase());
		}

		/// <summary>
		/// 
		/// </summary>
		public BlogPostEntity(string title)
		{
			Id = Guid.NewGuid();
			Title = title;
			Slug = Title.ToSlug();

			SetKeys(PartitionKeyString, FormatRowKey(Slug));
		}

		public BlogPostEntity() { }

		public Guid Id { get; set; }

		public string Slug { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Body { get; set; }

		public string Author { get; set; }

		public string Category { get; set; }

		public bool IsPublished { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}
