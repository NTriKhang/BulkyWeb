using Bulky.Areas.Admin.Controllers;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Bulky.Model.ViewMd;
using Bulky.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Stripe.Checkout;
using System.Security.Claims;

namespace Bulky.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class OrderController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public OrderController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index(List<CartVM> shoppingCartObj)
		{
            string UserID = shoppingCartObj[0].ApplicationUserId;
            var currentUser = _unitOfWork.applicationUser.GetFirstOrDefault(user => user.Id == UserID);
			OrderVM orderVM = new OrderVM()
			{
				orderHeader = new OrderHeader()
				{
					Name = currentUser.Name,
					PhoneNumber = currentUser.PhoneNumber,
					StreetAddress = currentUser.StreetAddress ?? string.Empty,
					City = currentUser.City ?? string.Empty,
					State = currentUser.State ?? string.Empty,
					PostalCode = currentUser.PostalCode ?? string.Empty,
					OrderTotal = shoppingCartObj[0].Total, 
				},
				orderDetail = new List<OrderDetail>(),
				cartId = new List<int>(),
			};
			foreach(var cart in shoppingCartObj)
			{

				OrderDetail orderDetailTmp = new OrderDetail()
				{
					ProductId = cart.Products.Id,
					Price = cart.Count * cart.Products.Price,
					Count = cart.Count,

				};
				orderVM.orderDetail.Add(orderDetailTmp);
				orderVM.cartId.Add(cart.shoppingCartId);
			}
			return View(orderVM);
		}
		[HttpPost]
		public IActionResult PlaceOrder(OrderVM orderVM)
		{
			orderVM.orderHeader.OrderDate = DateTime.Now;
			orderVM.orderHeader.OrderStatus = SD.StatusPending;
			orderVM.orderHeader.PaymentStatus = SD.PaymentStatusPending;
			orderVM.orderHeader.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

			_unitOfWork.OrderHeader.Add(orderVM.orderHeader);
			_unitOfWork.Save();

			var currentUser = _unitOfWork.applicationUser.GetFirstOrDefault(x => x.Id == orderVM.orderHeader.ApplicationUserId);
			if(currentUser.CompanyId.GetValueOrDefault() == 0)
			{
				var domain = "https://localhost:44391/";
				var options = new SessionCreateOptions
				{
					LineItems = new List<SessionLineItemOptions>(),
					Mode = "payment",
					SuccessUrl = domain + $"Customer/Order/OrderConfirm?id={orderVM.orderHeader.Id}",
					CancelUrl = domain + $"Customer/Order/OrderCancel?id={orderVM.orderHeader.Id}",
				};
				for (int i = 0; i < orderVM.cartId.Count; i++)
				{
					ShoppingCart2 cart = _unitOfWork.shoppingCart.GetFirstOrDefault(x => x.Id == orderVM.cartId[i]);
					Product product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == cart.ProductId);
					SessionLineItemOptions lineItems = new SessionLineItemOptions()
					{
						PriceData = new SessionLineItemPriceDataOptions()
						{
							UnitAmount = (long)(product.Price * 1000),
							Currency = "vnd",
							ProductData = new SessionLineItemPriceDataProductDataOptions()
							{
								Name = product.Title,
							},
						},
						Quantity = cart.Count,
					};
					options.LineItems.Add(lineItems);
					//
					orderVM.orderDetail[i].OrderId = orderVM.orderHeader.Id;
					_unitOfWork.OrderDetail.Add(orderVM.orderDetail[i]);
					_unitOfWork.Save();
				}
				var service = new SessionService();
				Session session = service.Create(options);
				_unitOfWork.OrderHeader.UpdateStripePaymentStatus(orderVM.orderHeader, session.Id, session.PaymentIntentId);
				_unitOfWork.Save();
				Response.Headers.Add("Location", session.Url);
				return new StatusCodeResult(303);
			}
			else
			{
				for(int i =0; i < orderVM.cartId.Count; i++)
				{
					orderVM.orderDetail[i].OrderId = orderVM.orderHeader.Id;
					_unitOfWork.OrderDetail.Add(orderVM.orderDetail[i]);
					_unitOfWork.Save();
				}
				_unitOfWork.OrderHeader.UpdateStatus(orderVM.orderHeader, SD.StatusApprove, SD.PaymentStatusDelayApprove);
				IEnumerable<ShoppingCart2> listCart = _unitOfWork.shoppingCart.GetAll(x => x.ApplicationUserId == orderVM.orderHeader.ApplicationUserId);
				_unitOfWork.shoppingCart.RemoveRange(listCart);
				_unitOfWork.Save();
				return View("OrderConfirm", orderVM.orderHeader.Id);
			}
			
		}
		public IActionResult OrderConfirm(int id)
		{
			var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == id);
			var service = new SessionService();
			Session session = service.Get(orderHeader.SessionId);
			if(session.PaymentStatus.ToLower() == "paid")
			{
				_unitOfWork.OrderHeader.UpdateStripePaymentStatus(orderHeader, orderHeader.SessionId, session.PaymentIntentId);
				_unitOfWork.OrderHeader.UpdateStatus(orderHeader, SD.StatusApprove, SD.PaymentStatusApprove);

			}
			List<ShoppingCart2> shoppingcarts = _unitOfWork.shoppingCart.GetAll().Where(x => x.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
			_unitOfWork.shoppingCart.RemoveRange(shoppingcarts);
			_unitOfWork.Save();
			return View(id);
		}
		public IActionResult OrderCancel(int id)
		{
			OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == id);
			List<OrderDetail> orderDetails = _unitOfWork.OrderDetail.GetAll().Where(x => x.OrderId == id).ToList();
			_unitOfWork.OrderDetail.RemoveRange(orderDetails);
			_unitOfWork.OrderHeader.Remove(orderHeader);
			_unitOfWork.Save();
			return RedirectToAction("Index", "Cart");
		}
	}
}
