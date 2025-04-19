using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using Builky.Models.ViewModels;
using Builky.Utility;
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

        public IActionResult Details(int orderId)
        {
            OrderVM orderVM = new()
            {
                OrderHeader = _unitOfWork.orderHeaderRespoitory.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                orderDetails = _unitOfWork.orderDetailsRespoitory.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };

            return View(orderVM);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders = _unitOfWork
                .orderHeaderRespoitory.GetAll(includeProperties: "ApplicationUser")
                .ToList();

            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u =>
                        u.PaymentStatus == SD.PaymentStatusDelayedPayment
                    );
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == SD.StatusApproved);
                    break;

                default:
                    break;
            }

            return Json(new { data = orderHeaders });
        }
        #endregion
    }
}
