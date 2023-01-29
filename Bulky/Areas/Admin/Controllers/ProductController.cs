using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Bulky.Model.ViewMd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using NuGet.Packaging.Signing;

namespace Bulky.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitofwork;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			_unitofwork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Upsert(int? id)
		{
			ProductVM productVM = new ProductVM()
			{
				product = new Product(),
				Category = _unitofwork.Category.GetAll().Select(selector => new SelectListItem
				{
					Text = selector.Name,
					Value = selector.Id.ToString(),
				}),
				CoverType = _unitofwork.CoverType.GetAll().Select(selector => new SelectListItem
				{
					Text = selector.Name,
					Value = selector.Id.ToString(),
				})
			};
			if (id == null)
			{
				return View(productVM);
			}
			else
			{
				productVM.product = _unitofwork.Product.GetFirstOrDefault(u => u.Id == id);
				return View(productVM);
			}

		}
		[HttpPost]
		public async Task<IActionResult> Upsert(ProductVM obj, IFormFile? file)
		{
			if (obj == null)
				return NotFound();
			if (ModelState.IsValid)
			{
				if (file != null)
				{
					var wwwRootPath = _webHostEnvironment.WebRootPath;
					var filename = Guid.NewGuid().ToString();
					var extension = Path.GetExtension(file.FileName);
					string fullpath = Path.Combine(wwwRootPath, "images\\products", filename + extension);
					if (obj.product.UrlImage != null)
					{
						var oldImage = Path.Combine(wwwRootPath, obj.product.UrlImage.TrimStart('\\'));
						if (System.IO.File.Exists(oldImage))
						{
							System.IO.File.Delete(oldImage);
						}
					}
					using (var filestream = new FileStream(fullpath, FileMode.Create))
					{
						await file.CopyToAsync(filestream);
					};
					obj.product.UrlImage = @"\images\products\" + filename + extension;
				}
				if (obj.product.Id == 0)
				{
					_unitofwork.Product.Add(obj.product);
				}
				else
				{
					_unitofwork.Product.Update(obj.product);
				}
				_unitofwork.Save();
				return RedirectToAction("Index");
			}
			return View(obj);
		}


		#region API CALLS   
		[HttpGet]
		public IActionResult GetAll()
		{
			var product = _unitofwork.Product.GetAll(includeProperty: "Category  , CoverType");
			return Json(new { data = product });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			if (id == null)
				return Json(new { success = false});

			var obj = _unitofwork.Product.GetFirstOrDefault(u => u.Id == id);
			var objImage = _webHostEnvironment.WebRootPath;
			string fullpath = objImage + obj.UrlImage.TrimStart('\\');
			if (System.IO.File.Exists(objImage + obj.UrlImage))
			{
				System.IO.File.Delete(objImage + obj.UrlImage);
			}

			_unitofwork.Product.Remove(obj);
			_unitofwork.Save();
			return Json(new { success = true });
		}
		#endregion

	}
}
