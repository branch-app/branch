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
				viewModel.InvitationCode = viewModel.InvitationCode.Trim();

				// Validate uniqueness of Username and Email
				var user = sqlStorage.BranchIdentities
					.FirstOrDefault(i => 
						i.Username.ToLower() == viewModel.Username.ToLower() ||
						i.Email.ToLower() == viewModel.Email.ToLower());
				if (user != null)
				{
					ModelState.AddModelError("Username", "Either this username has already been taken, or that email has already been used.");
					ModelState.AddModelError("Email", "Either this username has already been taken, or that email has already been used.");
				}

				// Validate Invite Code
				var invite =
					sqlStorage.BranchIdentityInvitations.FirstOrDefault(
						i => i.InvitationCode.ToLower() == viewModel.InvitationCode.ToLower() && !i.Used);
				if (invite == null)
					ModelState.AddModelError("InvitationCode", "This invite code has either been used or isn't valid. Sorry bae.");

				// Check Password is identical
				if (viewModel.Password != viewModel.PasswordConfirm)
					ModelState.AddModelError("Password", "Your password and confirmation do not match.");

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
					ModelState.AddModelError("Password", "Your password is not complex enough.");

				if (!ModelState.IsValid)
				{
					viewModel.Password = viewModel.PasswordConfirm = "";
					return View(viewModel);
				}

				// All gucci, create Branch Identity
				var password = Pbkdf2Crypto.ComputeHash(viewModel.Password, new Random().Next(1000, 1200));
				var branchIdentity = new BranchIdentity
				{
					BranchRole = sqlStorage.BranchRoles.First(r => r.Type == RoleType.User),
					Email = viewModel.Email,
					FullName = viewModel.FullName,
					Username = viewModel.Username,
					PasswordHash = password.Hash,
					PasswordIterations = password.Iterations,
					PasswordSalt = password.Salt,
					BranchIdentityInvitation = invite
				};

				// Set invite as used
// ReSharper disable once PossibleNullReferenceException
				invite.Used = true;

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

				return RedirectToRoute("BranchIdentityView", new { controller = "Home", action = "Index", slug = branchIdentity.Username });
			}

		}
	}
}