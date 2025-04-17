using Builky.DataAccess.Repository.IRepository;
using Builky.Models.Models;
using Builky.Models.ViewModels;
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
        public ShoppingCartVM cartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {


            var claimIdentity = (ClaimsIdentity) User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            cartVM = new()
            {
                shoppingCartList = _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == userId , includeProperties: "product")
            };

            foreach(var cart in cartVM.shoppingCartList)
            {
                cart.price = GetPriceBasedOnQuantity(cart);
                cartVM.orderTotal += (cart.price * cart.Count);
            }

            return View(cartVM);
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.product.Price;
            }

            else
            {
                if(shoppingCart.Count <= 100)
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
