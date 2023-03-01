using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using Bulky.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authorization;
using Bulky.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky.Model.ViewMd;

namespace Bulky.Areas.Admin.Controllers
{
    [Area("Admin")]
	
	public class AdminFeatureController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IEmailSender _emailSender;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public AdminFeatureController(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_unitOfWork = unitOfWork;
			_emailSender = emailSender;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		[HttpGet]
		[Authorize(Roles = SD.RoleUserAdmin)]
		public async Task<IActionResult> Index()
		{
			//var tmp = _unitOfWork.applicationUser.GetFirstOrDefault(x => x.Email == "nguyentrikhang1703@gmail.com");
			//_userManager.AddToRoleAsync(tmp, SD.RoleUserAdmin).GetAwaiter();
			//_unitOfWork.Save();
			var users = _unitOfWork.applicationUser.GetAll(x => x.Email != "nguyentrikhang1703@gmail.com");
			SignRoleVM signRoleVM = new SignRoleVM()
			{
				signRoles = new List<SignRole>(),
				roles = new List<SelectListItem>(),
			};
			foreach(var user in users)
			{
				var singrole = new SignRole
				{
					applicationUser = user,
					specificRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult()[0],
				};
				signRoleVM.signRoles.Add(singrole);
			}
			signRoleVM.roles.Add(new SelectListItem
			{
				Value = SD.RoleUserAdmin,
				Text = SD.RoleUserAdmin
			});
            signRoleVM.roles.Add(new SelectListItem
            {
                Value = SD.RoleUserEmp,
                Text = SD.RoleUserEmp
            });
			signRoleVM.roles.Add(new SelectListItem
			{
				Value = SD.RoleUserComp,
				Text = SD.RoleUserComp
            });
            signRoleVM.roles.Add(new SelectListItem
            {
                Value = SD.RoleUserIndi,
                Text = SD.RoleUserIndi
            });
            return View(signRoleVM);
		}
		public async Task<IActionResult> SignRolePost(SignRoleVM signRoleVM)
		{
			
			for(int i = 0; i < signRoleVM.signRoles.Count; i++)
			{
				var user = signRoleVM.signRoles[i].applicationUser;
				var role = signRoleVM.signRoles[i].specificRole;
				var previousRole = _userManager.GetRolesAsync(user).GetAwaiter().GetResult()[0];
				await _userManager.RemoveFromRoleAsync(user, previousRole);
                _unitOfWork.Save();
                await _userManager.AddToRoleAsync(user, role);
                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
		}
		public async Task<IActionResult> SendEmail()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var user = _unitOfWork.applicationUser.GetFirstOrDefault(x => x.Id == userId);

			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var returnUrl = Url.Action("PlaceOrder", "Order");
			code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
			var callbackUrl = Url.Page(
			"/Account/ConfirmEmail",
			pageHandler: null,
			values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
			protocol: Request.Scheme);
			var receiver = user.Email;
			await _emailSender.SendEmailAsync(receiver, "Confirm your email",
						$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
			return View();
		}
    }
}
