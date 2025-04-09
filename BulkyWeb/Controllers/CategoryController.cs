using Builky.DataAccess.Data;
using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;

using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRespoitory _categoryRespoitory;

        public CategoryController(ICategoryRespoitory categoryRespoitory)
        {
            _categoryRespoitory = categoryRespoitory;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _categoryRespoitory.GetAll().ToList();

            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name?.ToLower() == "test")
            {
                ModelState.AddModelError("", "Test Is Invalid value!");
            }


            if(category.Name?.ToLower() == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name","Display Order Cannot Exactly Match The Name");
            }

            if (ModelState.IsValid)
            {
                _categoryRespoitory.Add(category);
                _categoryRespoitory.save(); 
                TempData["Success"] = "Categrory Created Successfully";
                return RedirectToAction("Index", "Category");
            }
            return View(); 
        }


        public IActionResult Edit(int? id)
        {

            if(id is null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _categoryRespoitory.Get(u => u.CategoryId == id);
            if(categoryFromDb is null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRespoitory.Update(category);
                _categoryRespoitory.save();
                TempData["Success"] = "Categrory Updated Successfully";

                return RedirectToAction("Index");
            }

            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _categoryRespoitory.Get(u => u.CategoryId == id);
            if (categoryFromDb is null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }


        [HttpPost , ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {

            Category? category = _categoryRespoitory.Get(u => u.CategoryId == id);
            if (category is null)
            {
                return NotFound();
            }

            _categoryRespoitory.Remove(category);
            _categoryRespoitory.save();
            TempData["Success"] = "Categrory Deleted Successfully";

            return RedirectToAction("Index");
        }

    }
}
