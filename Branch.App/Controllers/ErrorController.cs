using System;
using System.Web.Mvc;
using Branch.App.Models;

namespace Branch.App.Controllers
{
	public class ErrorController : Controller
	{
		//
		// GET: /Error/
		public ActionResult Index(Exception exception, Guid? loggedExceptionGuid, 
			int? statusCode)
		{
			if (loggedExceptionGuid == null || statusCode == null)
				return RedirectToAction("Index", "Home");
			
			return View(new ErrorViewModel(exception, (Guid) loggedExceptionGuid, (int) statusCode));
		}
	}
}
