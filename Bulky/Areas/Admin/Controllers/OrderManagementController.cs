using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bulky.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize]
	public class OrderManagementController : Controller
	{
		private readonly IUnitOfWork _unitofwork;
		public OrderManagementController(IUnitOfWork unitofwork)
		{
			_unitofwork = unitofwork;
		}

		public IActionResult Index()
		{
			return View();
		}
		#region API
		[HttpGet]
		public IActionResult GetAll(string status)
		{
			IEnumerable<OrderHeader> orderHeader;
			if (User.IsInRole(SD.RoleUserAdmin) || User.IsInRole(SD.RoleUserEmp))
			{
				orderHeader = _unitofwork.OrderHeader.GetAll();
			}
			else
			{
				var currentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);
				orderHeader = _unitofwork.OrderHeader.GetAll(x => x.ApplicationUserId == currentUser);
			}
			if (status != "all")
			{
				orderHeader = orderHeader.Where(x => x.PaymentStatus.ToLower() == status);
			}
			return Json(new { data = orderHeader });
		}
		#endregion
	}
}
