﻿using System.Security.Claims;
using Builky.DataAccess.Repository.IRepository;
using Builky.Utility;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {

        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim is not null)
            {

                if (HttpContext.Session.GetInt32(SD.SessionCart) is null)
                {
                    HttpContext.Session.SetInt32(SD.SessionCart,
                     _unitOfWork.ShoppingCartRepository.GetAll(u => u.ApplicationUserId == claim.Value).Count());
                }
                return View(HttpContext.Session.GetInt32(SD.SessionCart));

            }

            else
            {
                HttpContext.Session.Clear();

                return View(0);
            }
        }

    }
}
