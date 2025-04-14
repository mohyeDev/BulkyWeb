using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using Builky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {


        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public IActionResult Index()
        {

            List<Company> CompanyList = _unitOfWork.companyReositiory.GetAll().ToList();

            return View(CompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            if (id is null || id <= 0)
            {
                return View(new Company());
            }

            else
            {
                Company company = _unitOfWork.companyReositiory.Get(u => u.Id == id);
                return View(company);
            }
        }

        [HttpPost]

        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.companyReositiory.Add(company);
                }

                else
                {
                    _unitOfWork.companyReositiory.Update(company);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";

                return RedirectToAction("Index");
            }

            else
            {
                return View(company);
            }
        }




        #region ApiCall

        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.companyReositiory.GetAll().ToList();
            return Json(new { data = companies });
        }


        [HttpDelete]

        public IActionResult Delete(int? id)
        {
            Company company = _unitOfWork.companyReositiory.Get(u => u.Id == id);
            if(company is null)
            {
                return Json(new { sucess = false , message = "Error While Deleting"});
            }

            _unitOfWork.companyReositiory.Remove(company);
            _unitOfWork.Save();

            return Json(new { sucess = true, message = "Delete Successfully!" });
        }
        #endregion
    }



}
