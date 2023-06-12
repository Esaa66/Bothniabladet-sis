using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Bothniabladet.Models
{
  public class RegisterViewModel
    {
    [Required(ErrorMessage = "Enter a username")]
    [Display(Name = "Username")]
    public String UserName { get; set; }

    [Required(ErrorMessage = "Enter email")]
    [Display(Name = "Email")]
    [EmailAddress]
    public String Email { get; set; }

    [StringLength(420, ErrorMessage = "The password must be at least 6 characters long", MinimumLength = 6)]
    [Required(ErrorMessage = "Enter a password")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public String Password { get; set; }
    [Required(ErrorMessage = "Repeat password")]
    [DataType(DataType.Password)]
    [Display(Name = "Repeat password")]
    [Compare("Password", ErrorMessage = "The solved words do not match")]
    public String ConfirmPassword { get; set; }

  }
}
