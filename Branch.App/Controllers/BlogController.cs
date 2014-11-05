using System.Linq;
using System.Web.Mvc;
using Branch.App.Helpers;
using Branch.Models.Services.Branch;

namespace Branch.App.Controllers
{
	public class BlogController : BaseController
	{
		// GET: /Blog/
		public ActionResult Index()
		{
			var blogPosts =
				GlobalStorage.AzureStorage.Table.RetrieveMultipleEntities<BlogPostEntity>(BlogPostEntity.PartitionKeyString,
					GlobalStorage.AzureStorage.Table.BranchCloudTable).Where(p => p.IsPublished).OrderByDescending(p => p.CreatedAt);

			return View(blogPosts);
		}

		// GET: /Blog/View/{id}
		public new ActionResult View(string id)
		{
			var blogPost =
				GlobalStorage.AzureStorage.Table.RetrieveMultipleEntities<BlogPostEntity>(BlogPostEntity.PartitionKeyString,
					GlobalStorage.AzureStorage.Table.BranchCloudTable).FirstOrDefault(p => p.Slug == id && p.IsPublished);

			if (blogPost != null) return View(blogPost);

			Response.StatusCode = 404;
			return null;
		}
	}
}
