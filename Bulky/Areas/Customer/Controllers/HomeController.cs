using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
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
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperty: "Category");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int cnt = _unitOfWork.shoppingCart.GetAll(x => x.ApplicationUserId == userId).ToList().Count;
            HttpContext.Session.SetInt32(SD.CartSession, cnt);
            //var receiver = "rokkhangnvd@gmail.com";
            //var subject = "Test";
            //var message = "<h2>Thank you for your e-mail</h2><font size=4>We appreciate your feedback.<br/> We will process your request soon</font><br/>Regards.";
            //await _emailSender.SendEmailAsync(receiver, subject, message);
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