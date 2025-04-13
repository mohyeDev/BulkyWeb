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
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> productsList = _unitOfWork.productRepository.GetAll(includeProperties: "Category").ToList();

            return View(productsList);
        }

        public IActionResult Upsert(int? id)
        {

            //ViewBag.CategoryList = CategoryList; 
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork
                .categoryRespoitory.GetAll()
                .Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.CategoryId.ToString(),
                }),
                Product = new Product()
            };

            if (id is null || id <= 0)
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
                if (file is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var productPAth = Path.Combine(wwRootPAth, @"Images\Product\");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //Delete The Old Image 
                        var oldImagePath = Path.Combine(wwRootPAth, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);

                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPAth, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    };

                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;

                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.productRepository.Add(productVM.Product);

                }
                else
                {
                    _unitOfWork.productRepository.Update(productVM.Product);

                }
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




    #region APICALLS
    [HttpGet]

        public IActionResult GetAll()
        {
            List<Product> productsList = _unitOfWork.productRepository.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = productsList });

        }

        public IActionResult Delete(int? id)
        {
            Product product = _unitOfWork.productRepository.Get(u => u.Id == id);

            if(product is null)
            {
                return Json(new { sucess = false , message ="Error While Deleting" }); 
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, product
                .ImageUrl.TrimStart('\\'));
            if(System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.productRepository.Remove(product);
            _unitOfWork.Save();
            return Json(new { sucess = true , message ="Deleted Successfully!"}); 
        }
        #endregion
    }
}