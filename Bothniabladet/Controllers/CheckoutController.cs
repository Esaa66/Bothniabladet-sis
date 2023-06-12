using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bothniabladet.Models.Checkout;
using Bothniabladet.Data;
using Microsoft.AspNetCore.Authorization;
using Bothniabladet.Services;
using Microsoft.AspNetCore.Identity;

namespace Bothniabladet.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly AppDbContext _context;
        public CheckoutService _service;

        public CheckoutController(AppDbContext context, CheckoutService service)
        {
            _context = context;
            _service = service;
        }

        // GET: /Checkout/AddressAndPayment
        public IActionResult AddressAndPayment()
        {
            var shoppingCart = _service.GetShoppingCart();
            return View(shoppingCart); // Shows items in the shopping cart
        }

        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddressAndPayment(int imageId)
        {
            bool removed = _service.RemoveItem(imageId); // Attempt to remove the item from the shopping cart
            if (!removed)
            {
                // Handle the case when the item was not found or failed to be removed
                // You can add appropriate error handling or display a message to the user
            }
            _service.AddItem(imageId); // Add the item to the shopping cart
            return RedirectToAction("AddressAndPayment"); // Redirect to the cart page
        }

        // POST: /Checkout/RemoveItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveItem(int imageId)
        {
            bool removed = _service.RemoveItem(imageId); // Attempt to remove the item from the shopping cart
            if (!removed)
            {
                // Handle the case when the item was not found or failed to be removed
                // You can add appropriate error handling or display a message to the user
            }
            return RedirectToAction("AddressAndPayment"); // Redirect to the cart page
        }

        // GET: /Checkout/Complete
        public IActionResult Complete()
        {
            return View();
        }

        // POST: /Checkout/FinalForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FinalForm()
        {
            _service.CompleteTransaction();
            return View();
        }
    }
}
