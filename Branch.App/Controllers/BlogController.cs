using System.Linq;
using System.Web.Mvc;
using Branch.Models.Services.Branch;

namespace Branch.App.Controllers
{
	public class BlogController : Controller
	{
		// GET: Blog
		public ActionResult Index()
		{
			var blogPosts =
				GlobalStorage.AzureStorage.Table.RetrieveMultipleEntities<BlogPostEntity>(BlogPostEntity.PartitionKeyString,
					GlobalStorage.AzureStorage.Table.BranchCloudTable).OrderByDescending(p => p.Timestamp);

			return View(blogPosts);
		}
	}
}
