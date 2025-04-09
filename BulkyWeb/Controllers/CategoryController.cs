using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categrories.ToList();

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
                _db.Categrories.Add(category);
                _db.SaveChanges();
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

            Category? categoryFromDb = _db.Categrories.Find(id);
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
                _db.Categrories.Update(category);
                _db.SaveChanges();
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

            Category? categoryFromDb = _db.Categrories.Find(id);
            if (categoryFromDb is null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }


        [HttpPost]
        public IActionResult DeletePOST(int? id)
        {

            Category category = _db.Categrories.Find(id);
            if(category is null)
            {
                return NotFound();
            }

            _db.Categrories.Remove(category);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
