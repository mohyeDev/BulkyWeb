using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using Builky.Models.ViewModels;
using Builky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM orderVM { get; set; }

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
             orderVM = new()
            {
                OrderHeader = _unitOfWork.orderHeaderRespoitory.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
                orderDetails = _unitOfWork.orderDetailsRespoitory.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
            };

            return View(orderVM);
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult StartProcessing()
        {
            _unitOfWork.orderHeaderRespoitory.UpdateStatus(orderVM.OrderHeader.Id,SD.StatusInProcess);
            _unitOfWork.Save();
            TempData["success"] = "Order Details Updated Successfully.";
            return RedirectToAction(nameof(Details), new {orderId = orderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]

        public IActionResult ShipOrder()
        {

            var orderHeader = _unitOfWork.orderHeaderRespoitory.Get(u => u.Id == orderVM.OrderHeader.Id);
            orderHeader.TrackingNumber  = orderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier  = orderVM.OrderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;

            if(orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment)
            {
                orderHeader.PaymentDueDate = DateOnly.FromDateTime( DateTime.Now.AddDays(30));
            }
            _unitOfWork.orderHeaderRespoitory.Update(orderHeader);

            _unitOfWork.orderHeaderRespoitory.UpdateStatus(orderVM.OrderHeader.Id, SD.StatusShipped);
            _unitOfWork.Save();
            TempData["success"] = "Order Shipped Successfully!";
            return RedirectToAction(nameof(Details), new { orderId = orderVM.OrderHeader.Id });
        }

        [HttpPost]
        [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult UpdateOrderDetail(int orderId)
        {

            var orderHeaderFromDb = _unitOfWork.orderHeaderRespoitory.Get(u => u.Id == orderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = orderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = orderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = orderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = orderVM. OrderHeader.City;
            orderHeaderFromDb.State = orderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = orderVM.OrderHeader.PostalCode;

            if (!string.IsNullOrEmpty(orderVM.OrderHeader.Carrier)) {

                orderHeaderFromDb.Carrier = orderVM.OrderHeader.Carrier;
            }


            if (!string.IsNullOrEmpty(orderVM.OrderHeader.TrackingNumber))
            {

                orderHeaderFromDb.TrackingNumber = orderVM.OrderHeader.TrackingNumber;
            }

            _unitOfWork.orderHeaderRespoitory.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["success"] = "Order Details Updated Successfully!";

            return RedirectToAction(nameof(Details), new { orderId = orderHeaderFromDb.Id });

        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork
                .orderHeaderRespoitory.GetAll(includeProperties: "ApplicationUser")
                .ToList();
            }


            else
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orderHeaders = _unitOfWork.orderHeaderRespoitory.GetAll(u => u.ApplicationUserId == userId,includeProperties: "ApplicationUser");
            }
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
