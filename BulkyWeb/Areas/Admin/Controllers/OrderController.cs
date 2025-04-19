using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS
        [HttpGet]

        public IActionResult GetAll()
        {
            List<OrderHeader> orderHeaders = _unitOfWork.orderHeaderRespoitory.GetAll(includeProperties: "ApplicationUser").ToList();
            return Json(new { data = orderHeaders });

        }
        #endregion



    }
}