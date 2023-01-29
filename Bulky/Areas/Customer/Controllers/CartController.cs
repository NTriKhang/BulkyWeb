using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Bulky.Model.ViewMd;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Xml;

namespace Bulky.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize]
	public class CartController : Controller
	{
		private readonly IUnitOfWork _unitofwork;

		public CartController(IUnitOfWork unitofwork)
		{
			_unitofwork = unitofwork;
		}

		public IActionResult Index()
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

			List<CartVM> cartVMs= new List<CartVM>();
			var shoppingCart = _unitofwork.shoppingCart.GetAll();
			foreach(var shop in shoppingCart)
			{
				if(userId == shop.ApplicationUserId)
				{
					cartVMs.Add(new CartVM()
					{
						shoppingCartId = shop.Id,
						Products = _unitofwork.Product.GetFirstOrDefault(x => x.Id == shop.ProductId),
						Count = shop.Count,
					});
				}
			}
			return View(cartVMs);
		}

		public IActionResult Delete(int? id)
		{
			if (id == null)
				return Json(new { success = false });
			var obj = _unitofwork.shoppingCart.GetFirstOrDefault(x => x.Id == id);
			_unitofwork.shoppingCart.Remove(obj);
			_unitofwork.Save();
			return Json(new {success = true});	
		}
	}
}
