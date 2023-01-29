using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _uniofwork;
        public CoverTypeController(IUnitOfWork uniofwork)
        {
            _uniofwork = uniofwork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> obj = _uniofwork.CoverType.GetAll();
            return View(obj);
        }

        public IActionResult Create()
        {
            var coverType = new CoverType();
            return View(coverType);
        }
        [HttpPost]
        public IActionResult Create(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _uniofwork.CoverType.Add(coverType);
                _uniofwork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            var obj = _uniofwork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        public IActionResult Edit(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _uniofwork.CoverType.Update(coverType);
                _uniofwork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            var obj = _uniofwork.CoverType.GetFirstOrDefault(x => x.Id == id);
            if (obj == null)
                return NotFound();
            return View(obj);
        }
        [HttpPost]
        public IActionResult Delete(CoverType coverType)
        {
            _uniofwork.CoverType.Remove(coverType);
            _uniofwork.Save();
            return RedirectToAction("Index");
        }
    }
}
