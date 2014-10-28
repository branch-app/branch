using System.Web.Mvc;
using Branch.App.Helpers.Attributes;
using Branch.App.Helpers.Mvc;
using Branch.Models.Sql;

namespace Branch.App.Areas.Domain.Controllers
{
	public class HomeController : Controller
	{
		// GET: Domain/Home
		[RequiresAuthentication(RoleType.Administrator)]
		public ActionResult Index()
		{
			return SafeJsonContent.Create(new { note = "This view needs work. lol."});
		}
	}
}