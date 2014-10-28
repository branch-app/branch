using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Branch.App.Areas.Identity.Models;
using Branch.App.Helpers.Attributes;
using Branch.Core.Cryptography;
using Branch.Core.Storage;
using Branch.Models.Sql;

namespace Branch.App.Areas.Identity.Controllers
{
	public class CreateController : Controller
	{
		// GET: Identity/Create
		[RequiresNoAuthentication]
		public ActionResult Index()
		{
			return View(new CreateIdentityViewModel());
		}

		// POST: Identity/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[RequiresNoAuthentication]
		public ActionResult Index(CreateIdentityViewModel viewModel)
		{
			using (var sqlStorage = new SqlStorage())
			{
				if (!ModelState.IsValid) return View(viewModel);

				// Trimmin'
				viewModel.Email = viewModel.Email.Trim();
				viewModel.FullName = viewModel.FullName.Trim();
				viewModel.Gamertag = viewModel.Gamertag.Trim();
				viewModel.Username = viewModel.Username.Trim();

				// Validate uniqueness or email and username
				var user = sqlStorage.BranchIdentities.FirstOrDefault(i => i.Username.ToLower() == viewModel.Username.ToLower());
				if (user != null)
				{
					ModelState.AddModelError("Username", "The specified username has already been taken.");
					return View(viewModel);
				}
				user = sqlStorage.BranchIdentities.FirstOrDefault(i => i.Email.ToLower() == viewModel.Email.ToLower());
				if (user != null)
				{
					ModelState.AddModelError("Email", "The specified email has already been taken.");
					return View(viewModel);
				}

				// Check Password is identical
				if (viewModel.Password != viewModel.PasswordConfirm)
				{
					ModelState.AddModelError("Password", "Your password and confirmation do not match.");
					return View(viewModel);
				}

				// Check Password Complexity
				var complexity = 0;
				if (Regex.IsMatch(viewModel.Password, @"\d+"))
					complexity++;
				if (Regex.IsMatch(viewModel.Password, @"[a-z]+"))
					complexity++;
				if (Regex.IsMatch(viewModel.Password, @"[A-Z]+"))
					complexity++;
				if (Regex.IsMatch(viewModel.Password, @"[^a-zA-Z\d]+"))
					complexity++;

				if (complexity < 2)
				{
					ModelState.AddModelError("Password", "Your password is not complex enough.");
					return View(viewModel);
				}

				// All gucci, create Branch Identity
				var password = Pbkdf2Crypto.ComputeHash(viewModel.Password, new Random().Next(1000, 2000));
				var branchIdentity = new BranchIdentity
				{
					BranchRole = sqlStorage.BranchRoles.First(r => r.Type == RoleType.User),
					Email = viewModel.Email,
					FullName = viewModel.FullName,
					Username = viewModel.Username,
					PasswordHash = password.Hash,
					PasswordIterations = password.Iterations,
					PasswordSalt = password.Salt
				};

				// Check gamer ids
				GlobalStorage.H4Manager.GetPlayerServiceRecord(viewModel.Gamertag, true);
				GlobalStorage.HReachManager.GetPlayerServiceRecord(viewModel.Gamertag, true);
				var gamerIdSafe = GamerIdentity.EscapeGamerId(viewModel.Gamertag);
				var gamerId = sqlStorage.GamerIdentities.FirstOrDefault(g => g.GamerIdSafe == gamerIdSafe);
				if (gamerId != null)
					branchIdentity.GamerIdentity = gamerId;
				sqlStorage.BranchIdentities.Add(branchIdentity);
				sqlStorage.SaveChanges();

				var branchSession = BranchSession.Create(Request.UserHostAddress, Request.UserAgent, branchIdentity, false);
				sqlStorage.BranchSessions.Add(branchSession);

				var cookie = new HttpCookie("SessionIdentifier", branchSession.Identifier.ToString())
				{
					Expires = branchSession.ExpiresAt
				};
				Response.SetCookie(cookie);
				sqlStorage.SaveChanges();

				// TODO: Redirect to account page
				return RedirectToRoute("Welcome");
			}

		}
	}
}