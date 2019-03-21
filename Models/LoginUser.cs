using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccount.Models
{
    public class LoginUser
    {

        [Required(ErrorMessage="Email is required!")]
        [EmailAddress(ErrorMessage="Please enter a valid email address!")]
        [Display(Name="Email:")]
        public string Email {get; set;}

        [Required(ErrorMessage="Password is required!")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters long!")]
        [Display(Name="Password:")]
        [DataType(DataType.Password)]
        public string Password {get; set;}
    }
}