using Bulky.DataAccess;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Model;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _unitOfWork.Category.GetAll();
            return View(categories);
        }
        public IActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            category.CreatedDate = DateTime.Now;
            if (category.DisplayOrder < 1)
            {
                ModelState.AddModelError("DisplayOrder", "Display Order cannot lower than 1");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            Category category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (category == null)
                return NotFound();
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (category.DisplayOrder < 1)
            {
                ModelState.AddModelError("DisplayOrder", "Display Order cannot lower than 1");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();
            Category category = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            if (category == null)
                return NotFound();
            return View(category);
        }
        [HttpPost]
        public IActionResult Delete(Category category)
        {
            if (category.Id == null)
            {
                return RedirectToAction("Index");
            }
            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
