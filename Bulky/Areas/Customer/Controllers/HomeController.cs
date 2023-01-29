using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;

namespace Bulky.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperty: "Category");
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
		public IActionResult Detail(int? productId)
		{
            ShoppingCart2 shoppingCart = new ShoppingCart2()
            {
                product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == productId, includeProperty: "Category,CoverType"),
				Count = 1,
			};
			return View(shoppingCart);
		}
        [HttpPost]
        [Authorize]
		public IActionResult Detail(ShoppingCart2 shoppingCart)
		{

			shoppingCart.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
            _unitOfWork.shoppingCart.Add(shoppingCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
		}
	}
}