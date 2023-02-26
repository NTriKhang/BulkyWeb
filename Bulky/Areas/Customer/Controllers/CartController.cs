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
			int price = 0;
			int i = 0;
			foreach(var shop in shoppingCart)
			{
				if(userId == shop.ApplicationUserId)
				{
					cartVMs.Add(new CartVM()
					{
						ApplicationUserId = userId,
						shoppingCartId = shop.Id,
						Products = _unitofwork.Product.GetFirstOrDefault(x => x.Id == shop.ProductId),
						Count = shop.Count,
					});
					price += cartVMs[i].Count * cartVMs[i].Products.Price;
					i++;
				}

			}
			if (cartVMs.Count > 0)
			{
				cartVMs[0].Total = price;
				cartVMs[0].emailcofirm = _unitofwork.applicationUser.GetFirstOrDefault(x => x.Id == userId).EmailConfirmed;
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
		
		public IActionResult Update(List<CartVM> shoppingCartObj)
		{
			
			//if(ModelState.IsValid)
			//{
			//	for (int i = 0; i < shoppingCart2.Count; i++)
			//	{
			//		ShoppingCart2 tmp = _unitofwork.shoppingCart.GetFirstOrDefault(x => x.Id == shoppingCart2[i].shoppingCartId);
			//		tmp.Count = shoppingCart2[i].Count;
			//	}
			//	_unitofwork.Save();
			//	return RedirectToAction("index");
			//}
            if (ModelState.IsValid)
            {
                List<ShoppingCart2> obj = _unitofwork.shoppingCart.GetAll().Where(x => x.ApplicationUserId == shoppingCartObj[0].ApplicationUserId).ToList();
                for (int i = 0; i < shoppingCartObj.Count; i++)
                {
					obj[i].Count = shoppingCartObj[i].Count;
                }
                _unitofwork.Save();
                return RedirectToAction("index");
            }
            else
			{
				return View();
			}
		}
	}
}
