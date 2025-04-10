using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> productsList = _unitOfWork.productRepository.GetAll().ToList();
            return View(productsList);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]

        public IActionResult Create(Product product)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.productRepository.Add(product);
                _unitOfWork.Save();
                TempData["Success"] = "Product Created Successfully!";
                return RedirectToAction("Index");
            }

            return View();
        }


        public IActionResult Edit(int? id)
        {
            if(id is null || id <= 0)
            {
                return NotFound();
            }

            Product productFromDb = _unitOfWork.productRepository.Get(u => u.Id == id);
            if(productFromDb is null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        [HttpPost]

        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.productRepository.Update(product);
                _unitOfWork.Save();
                TempData["Success"] = "Product Updated Successfully";
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

            Product productFromDb = _unitOfWork.productRepository.Get(u => u.Id == id);

            if(productFromDb is null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        [HttpPost,ActionName("Delete")]

        public IActionResult DeletePOST(int? id)
        {

            Product product = _unitOfWork.productRepository.Get(u => u.Id == id);

            if(product is null)
            {
                return NotFound();
            }

            _unitOfWork.productRepository.Remove(product);

            _unitOfWork.Save();

            TempData["Success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");

        }
    }
}
