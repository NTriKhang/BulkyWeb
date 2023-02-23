using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = SD.RoleUserAdmin)]

	public class CompanyController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		public CompanyController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Upsert(int? id)
		{
			var obj = new Company();
			if (id == null)
				return View(obj);
			else
			{
				obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
				return View(obj);
			}
		}
		[HttpPost]
		public IActionResult Upsert(Company? company)
		{
			if (company == null)
				return NotFound();
			if (ModelState.IsValid)
			{
				if(company.Id == 0)
				{
					_unitOfWork.Company.Add(company);
				}
				else
				{
					_unitOfWork.Company.Update(company);
				}
				_unitOfWork.Save();
				return RedirectToAction("Index");
			}
			return View();
		}


		#region API CALL
		public IActionResult GetAll()
		{
			var obj = _unitOfWork.Company.GetAll();
			return Json(new { data = obj });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			if (id == null)
				return Json(new { success = false });
			var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
			_unitOfWork.Company.Remove(obj);
			_unitOfWork.Save();
			return Json(new { success = true });
		}
		#endregion
	}
}
