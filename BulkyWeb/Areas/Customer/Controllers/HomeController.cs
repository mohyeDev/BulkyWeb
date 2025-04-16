using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BulkyWeb.Models;
using Builky.DataAccess.Repository;
using Builky.Models.Models;
using Builky.DataAccess.Repository.IRepository;

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
