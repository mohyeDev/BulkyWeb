using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using Builky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> productsList = _unitOfWork.productRepository.GetAll().ToList();
            
            return View(productsList);
        }

        public IActionResult Upsert(int? id)
        {

            //ViewBag.CategoryList = CategoryList; 
            ProductVM productVM = new()
            {
                CategoryList =  _unitOfWork
                .categoryRespoitory.GetAll()
                .Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString(),
                }),
                Product = new Product()
            };

            if(id is null || id <= 0 )
            {

                //Create
                return View(productVM);
            }

            else
            {
                // Update
                productVM.Product = _unitOfWork.productRepository.Get(u => u.Id == id);
                return View(productVM);
                    
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                string wwRootPAth = _webHostEnvironment.WebRootPath;
                if(file is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var productPAth = Path.Combine(wwRootPAth, @"images\Product");

                    using (var fileStream = new FileStream(Path.Combine(productPAth, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    };

                    productVM.Product.ImageUrl = @"\images\Product" + fileName;

                }
                _unitOfWork.productRepository.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["Success"] = "Product Created Successfully!";
                return RedirectToAction("Index");
            }

            else
            {
                productVM.CategoryList = _unitOfWork.categoryRespoitory.GetAll().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString()
                });

                return View(productVM);

            }
        }

     

        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0)
            {
                return NotFound();
            }

            Product productFromDb = _unitOfWork.productRepository.Get(u => u.Id == id);

            if (productFromDb is null)
            {
                return NotFound();
            }

            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product product = _unitOfWork.productRepository.Get(u => u.Id == id);

            if (product is null)
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
