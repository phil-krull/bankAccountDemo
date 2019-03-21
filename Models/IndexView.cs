using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccount.Models
{
    public class IndexView
    {
        public Transaction Transaction {get; set;}
        public User User {get; set;}
    }
}