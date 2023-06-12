using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Bothniabladet.Data;
using Bothniabladet.Models.Checkout;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bothniabladet.Services
{
    public class CheckoutService
    {
        //Fetch the current user
        private readonly IHttpContextAccessor _httpContextAccessor;
        AppDbContext _context;
        readonly ILogger _logger;
        //constructor
        public CheckoutService(AppDbContext context, ILoggerFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = factory.CreateLogger<ImageService>();
            _httpContextAccessor = httpContextAccessor;
        }

        // Loads active users shoppingcart
        public ICollection<ShoppingCartModel> GetShoppingCart()
        {
            ApplicationUser user = _context.ApplicationUsers
                .Where(user => user.Id == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Include(user => user.ShoppingCart)
                    .ThenInclude(sc => sc.Image)
                .SingleOrDefault();

            ICollection<ShoppingCartModel> shoppingCartModels = new List<ShoppingCartModel>();
            int itemId = 1;
            foreach (ShoppingCart sc in user.ShoppingCart)
            {
                shoppingCartModels.Add(new ShoppingCartModel()
                {
                    ItemId = itemId++, // Assign the ID of the shopping cart item
                    Images = sc.Image,
                    Price = sc.Image.BasePrice,
                    User = sc.User,
                    ImagesStringData = sc.Image.Thumbnail == null
                        ? "default_value" // provide default value or image
                        : string.Format("data:image/jpg;base64,{0}", Convert.ToBase64String(sc.Image.Thumbnail))
                });
            }


            return shoppingCartModels;
        }
        //Adds a item to the shoppingcart, if owned true item does not exist in shoppingcart
        public void AddItem(int id)
        {
            Image image = _context.Images.Find(id);
            ApplicationUser user = _context.ApplicationUsers
                .Where(user => user.Id == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Include(user => user.ShoppingCart)
                .SingleOrDefault();

            if (user.ShoppingCart == null)
            {
                user.ShoppingCart = new List<ShoppingCart>();
            }
            bool inCart = false;
            foreach (ShoppingCart sc in user.ShoppingCart)
            {
                if (sc.ImageId == image.ImageId)
                {
                    inCart = true;
                }
            }
            if (!inCart)
            {
                user.ShoppingCart.Add(new ShoppingCart()
                {
                    Image = image,
                    User = user,
                    Owns = false
                });
            }

            _context.SaveChanges();
        }

        public bool RemoveItem(int imageId)
        {
            ApplicationUser user = _context.ApplicationUsers
                .Where(user => user.Id == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Include(user => user.ShoppingCart)
                .SingleOrDefault();


            ShoppingCart itemToRemove = user.ShoppingCart.FirstOrDefault(sc => sc.ImageId == imageId);

            if (itemToRemove != null)
            {
                user.ShoppingCart.Remove(itemToRemove);
                _context.SaveChanges();
                return true;
            }
            else
            {
                _logger.LogWarning($"Item with imageId {imageId} was not found in the shopping cart.");
                return false;
            }
        }




        public void CompleteTransaction()
        {

        }
    }
}
