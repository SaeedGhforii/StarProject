using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSite.EndPoint.Utilities;

namespace WebSite.EndPoint.Areas.Customers.Controllers
{
    [Authorize]
    [Area("Customers")]
    public class AddressController : Controller
    {
        private readonly IUserAddressService userAddressService;

        public AddressController(IUserAddressService userAddressService)
        {
            this.userAddressService = userAddressService;
        }
        public IActionResult Index()
        {
            var addresses = userAddressService.GetAddress(ClaimUtility.GetUserId(User));
            return View(addresses);
        }

        public IActionResult AddNewAddress()
        {
            return View(new AddUserAddressDto());
        }

        [HttpPost]
        public IActionResult AddNewAddress(AddUserAddressDto address)
        {
            string userId = ClaimUtility.GetUserId(User);
            address.UserId = userId;
            userAddressService.AddnewAddress(address);
            return RedirectToAction(nameof(Index));
        }
    }
}
