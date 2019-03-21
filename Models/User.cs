using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccount.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}

        [Required(ErrorMessage="First Name is required!")]
        [Display(Name="First Name:")]
        public string FirstName {get; set;}

        [Required(ErrorMessage="Last Name is required!")]
        [Display(Name="Last Name:")]
        public string LastName {get; set;}

        [Required(ErrorMessage="Email is required!")]
        [EmailAddress(ErrorMessage="Please enter a valid email address!")]
        [Display(Name="Email:")]
        public string Email {get; set;}

        [Required(ErrorMessage="Password is required!")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters long!")]
        [Display(Name="Password:")]
        [DataType(DataType.Password)]
        public string Password {get; set;}

        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords must match")]
        [Display(Name="Confirm Password:")]
        [DataType(DataType.Password)]
        public string Confirm_Password {get; set;}

        public List<Transaction> Transactions {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}