using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using BankAccount.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAccount.Controllers
{
    public class HomeController : Controller
    {
        private Context _context;
        public HomeController(Context context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Register()
        {
            HttpContext.Session.SetInt32("UserId", 1);
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult CreateUser(User User)
        {
            User CheckUser = _context.Users.FirstOrDefault(u => u.Email == User.Email);
            if(CheckUser != null)
            {
                ModelState.AddModelError("Email", "Email is already in use!");
            }
            if(ModelState.IsValid)
            {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User.Password = Hasher.HashPassword(User, User.Password);
                _context.Add(User);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", User.UserId);
                return RedirectToAction("Index", new{user_id=User.UserId});
            } else {
                return View("Register");
            }
        }

        [HttpPost("login")]
        public IActionResult LoginUser(LoginUser LoginUser)
        {
            User User = _context.Users.FirstOrDefault(u => u.Email == LoginUser.Email);
            if(User == null)
            {
                ModelState.AddModelError("Email", "Invalid Email!");
            }
            if(ModelState.IsValid == false)
            {
                return View("Login");
            } else {
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                PasswordVerificationResult Result = Hasher.VerifyHashedPassword(User, User.Password, LoginUser.Password);
                if(Result == 0)
                {
                    ModelState.AddModelError("Password", "Invalid Email/Password Combination");
                    return View("Login");
                } else {
                    HttpContext.Session.SetInt32("UserId", User.UserId);
                    return RedirectToAction("Index", new {user_id = User.UserId});
                }
            }
        }

        [HttpGet("account/{user_id}")]
        public IActionResult Index(int user_id)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                TempData["InvalidUser"] = "Must be logged in to continue";
                return RedirectToAction("Login");
            }

            if(user_id != (int)UserId)
            {
                TempData["InvalidUser"] = "You can only access your own account";
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            User User = _context.Users.Include(u => u.Transactions).FirstOrDefault(u => u.UserId == UserId);
            IndexView IndexView = new IndexView()
            {
                User = User
            };
            return View(IndexView);
        }

        [HttpPost("transaction")]
        public IActionResult Transaction(Transaction Transaction)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                TempData["InvalidUser"] = "Must be logged in to continue";
                return RedirectToAction("Login");
            }

            User User = _context.Users.Include(u => u.Transactions).FirstOrDefault(u => u.UserId == UserId);
            decimal Balance = User.Transactions.Sum(transaction => transaction.Amount);

            if(Balance < 0 || Balance + Transaction.Amount < 0)
            {
                ModelState.AddModelError("Transaction.Amount", "You have insufficient funds in your account");
            }
            if(Transaction.Amount == 0)
            {
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
                ModelState.Remove("Transaction.Amount");
                ModelState.AddModelError("Transaction.Amount", "Amount is required!");
            }
            if(ModelState.IsValid == false)
            {
                IndexView IndexView = new IndexView()
                {
                    User = User
                };
                return View("Index", IndexView);
            } else {
                Transaction.UserId = (int)UserId;
                _context.Add(Transaction);
                _context.SaveChanges();
                return RedirectToAction("Index", new {user_id = (int)UserId});
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Register");
        }
    }
}
