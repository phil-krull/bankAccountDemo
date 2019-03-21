using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccount.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId {get; set;}

        [Column(TypeName="decimal(10,2)")]
        [DataType(DataType.Currency)]
        [Range(-99999999, 99999999, ErrorMessage="Maxium allowed deposit/withdrawal exceeded!")]
        [Display(Name="Deposit/Withdraw:")]
        public decimal Amount {get; set;}
        public int UserId {get; set;}
        public User User {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;
        public DateTime UpdatedAt {get; set;} = DateTime.Now;
    }
}