using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Bothniabladet.Models
{
  public class LoginModel
  {
    [Required(ErrorMessage = "Please fill in your Email")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter password")]
    [Display(Name = "Password")]
    [UIHint("password")]
    public string Password { get; set; }

    public string ReturnUrl { get; set; }
  }
}
