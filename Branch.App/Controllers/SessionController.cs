using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Branch.App.Areas.Identity.Models;
using Branch.App.Helpers;
using Branch.App.Helpers.Attributes;
using Branch.Core.Cryptography;
using Branch.Core.Storage;
using Branch.Models.Sql;

namespace Branch.App.Controllers
{
	public class SessionController : BaseController
	{
		// GET: Session/Create
		[RequiresNoAuthentication]
		public ActionResult Create()
		{
			return View(new CreateSessionViewModel { RememberMe = true });
		}

		// POST: Session/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[RequiresNoAuthentication]
		public ActionResult Create(CreateSessionViewModel viewModel)
		{
			if (!ModelState.IsValid) return View(viewModel);

			using (var sqlStorage = new SqlStorage())
			{
				// trimmin
				viewModel.Username = (viewModel.Username ?? "").Trim();

				var branchIdentity =
					sqlStorage.BranchIdentities.FirstOrDefault(
						i => i.Email.ToLower() == viewModel.Username.ToLower() || i.Username.ToLower() == viewModel.Username.ToLower());
				if (branchIdentity == null)
				{
					ModelState.AddModelError("Username", "An Identity with that Username/Email doesn't exist.");
					return View(viewModel);
				}

				if (!Pbkdf2Crypto.ValidateHash(viewModel.Password, branchIdentity.PasswordHash, branchIdentity.PasswordSalt, branchIdentity.PasswordIterations))
				{
					ModelState.AddModelError("Password", "Incorrect Password.");
					return View(viewModel);
				}

				// Create Session
				var ipAddress = Request.ServerVariables.Get("HTTP_CF_CONNECTING_IP") ?? Request.UserHostAddress;
				var branchSession = BranchSession.Create(ipAddress, Request.UserAgent, branchIdentity, viewModel.RememberMe);
				sqlStorage.BranchSessions.Add(branchSession);
				branchIdentity.BranchIdentitySessions.Add(branchSession);

				// Set Cookie
				var cookie = new HttpCookie("SessionIdentifier", branchSession.Identifier.ToString())
				{
					Expires = branchSession.ExpiresAt
				};
				Response.SetCookie(cookie);
				sqlStorage.SaveChanges();

				return RedirectToRoute("BranchIdentityView", new { controller = "Home", action = "Index", slug = branchIdentity.Username });
			}
		}

		// GET: Session/Destroy
		[RequiresAuthentication]
		public ActionResult Destroy()
		{
			using (var sqlStorage = new SqlStorage())
			{
				var sessionIdentifier = Request.Cookies["SessionIdentifier"];
				if (sessionIdentifier != null)
				{
					var sessionGuid = Guid.Parse(sessionIdentifier.Value);
					var session = sqlStorage.BranchSessions.Include(s => s.BranchIdentity).FirstOrDefault(s => s.Identifier == sessionGuid);
					if (session != null)
					{
						sqlStorage.BranchSessions.First(s => s.Id == session.Id).Revoked = true;
						sqlStorage.SaveChanges();
					}
				}

				var myCookie = new HttpCookie("SessionIdentifier", "girl comes up to me and says \"what you drive?\" and i said \"BUGATTI\"")
				{
					Expires = DateTime.UtcNow.AddDays(-69d)
				};
				Response.Cookies.Add(myCookie);

				return RedirectToRoute("Welcome");
			}
		}
	}
}