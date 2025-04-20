using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BulkyWeb.Models;
using Builky.DataAccess.Repository;
using Builky.Models.Models;
using Builky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Builky.Utility;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        var claimIdentity = (ClaimsIdentity) User.Identity;
        var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

        if(claim.Value is not null)
        {
            HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value).Count());
        }





        IEnumerable<Product> productList = _unitOfWork.productRepository.GetAll(includeProperties:"Category");
        return View(productList);
    }

    public IActionResult Details(int id)
    {
        ShoppingCart shoppingCart = new()
        {
            product = _unitOfWork.productRepository.Get(u => u.Id == id, includeProperties: "Category"),
            Count = 1,
            ProductId = id

        };

        return View(shoppingCart);
    }


    [HttpPost]

    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        shoppingCart.Id = 0;
         var claimsIdentity  = (ClaimsIdentity) User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCart.ApplicationUserId = userId;
        ShoppingCart shoppingCartFromDb = _unitOfWork.ShoppingCartRepository.Get(u => u.ApplicationUserId == userId &&
        u.ProductId == shoppingCart.ProductId);
        if (shoppingCartFromDb is not null)
        {
            shoppingCartFromDb.Count += shoppingCart.Count;
            _unitOfWork.ShoppingCartRepository.Update(shoppingCartFromDb);
        }

        else
        {
            _unitOfWork.ShoppingCartRepository.Add(shoppingCart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart,
                _unitOfWork.ShoppingCartRepository.GetAll(u=>u.ApplicationUserId == userId)
                .Count());

        }

        TempData["success"] = "Cart Updated Sucessfully";
        return RedirectToAction(nameof(Index));

    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
