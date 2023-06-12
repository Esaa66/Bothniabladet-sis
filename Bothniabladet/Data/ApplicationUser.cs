using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Bothniabladet.Data
{
    //Custom data for the IdentityUser class, inherits IdentityUser
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ShoppingCart> ShoppingCart { get; set; } // A users check to see if they own any images
        public ICollection<UserDocuments> UserDocuments{ get; set; }
    }
}
