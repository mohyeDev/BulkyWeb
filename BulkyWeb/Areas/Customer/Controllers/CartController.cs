using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using Builky.Models.ViewModels;
using Builky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers
{

    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM cartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {


            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            cartVM = new()
            {
                shoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId, includeProperties: "product"),
                OrderHeader = new()
            };

            foreach (var cart in cartVM.shoppingCartList)
            {
                cart.price = GetPriceBasedOnQuantity(cart);
                cartVM.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }

            return View(cartVM);
        }


        public IActionResult Summary()
        {

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            cartVM = new()
            {
                shoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId, includeProperties: "product"),
                OrderHeader = new()
            };

            cartVM.OrderHeader.ApplicationUser = _unitOfWork.applicationUserRepository.Get(u => u.Id == userId);

            cartVM.OrderHeader.Name = cartVM.OrderHeader.ApplicationUser.Name;
            cartVM.OrderHeader.PhoneNumber = cartVM.OrderHeader.ApplicationUser.PhoneNumber;
            cartVM.OrderHeader.StreetAddress = cartVM.OrderHeader.ApplicationUser.StreetAddress;
            cartVM.OrderHeader.City = cartVM.OrderHeader.ApplicationUser.City;
            cartVM.OrderHeader.State = cartVM.OrderHeader.ApplicationUser.State;
            cartVM.OrderHeader.PostalCode = cartVM.OrderHeader.ApplicationUser.PostalCode;


            foreach (var cart in cartVM.shoppingCartList)
            {
                cart.price = GetPriceBasedOnQuantity(cart);
                cartVM.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }

            return View(cartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            cartVM.shoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product");

            cartVM.OrderHeader.OrderDate = System.DateTime.Now;

            cartVM.OrderHeader.ApplicationUserId = userId;

            cartVM.OrderHeader.ApplicationUser = _unitOfWork.applicationUserRepository.Get(u => u.Id == userId);


            foreach (var cart in cartVM.shoppingCartList)
            {
                cart.price = GetPriceBasedOnQuantity(cart);
                cartVM.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }

            if (cartVM.OrderHeader.ApplicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                cartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                cartVM.OrderHeader.OrderStatus = SD.StatusPending;
            }

            else
            {
                cartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                cartVM.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _unitOfWork.orderHeaderRespoitory.Add(cartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in cartVM.shoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = cartVM.OrderHeader.Id,
                    Price = cart.price,
                    Count = cart.Count
                };
                _unitOfWork.orderDetailsRespoitory.Add(orderDetail);
                _unitOfWork.Save();

            }
            return View(cartVM);
        }
        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(x => x.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCartRepository.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(x => x.Id == cartId);

            if (cartFromDb.Count <= 1)
            {
                //Remove

                _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            }

            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCartRepository.Update(cartFromDb);

            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCartRepository.Get(x => x.Id == cartId);
            _unitOfWork.ShoppingCartRepository.Remove(cartFromDb);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.product.Price;
            }

            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.product.Price50;
                }

                else
                {
                    return shoppingCart.product.Price100;
                }
            }
        }
    }
}
