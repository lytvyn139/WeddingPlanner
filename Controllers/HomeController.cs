using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
    
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        private User GetUser()
        {
          return dbContext.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("userId"));
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

      [HttpGet("registration")]
        public IActionResult Registration()
        {
             return View("registration");
        }
        
        ///////////////////////////////////
        //    LOGIN
        //////////////////////////////////

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
          // Check initial ModelState
          if(ModelState.IsValid)
          {
              // If a User exists with provided email
              if(dbContext.Users.Any(u => u.Email == user.Email))
              {
                  // Manually add a ModelState error to the Email field, with provided
                  // error message
                  ModelState.AddModelError("Email", "Email already in use!");
                  // You may consider returning to the View at this point
                  return View("registration");
              }
              PasswordHasher<User> Hasher = new PasswordHasher<User>();
              user.Password = Hasher.HashPassword(user, user.Password);
              dbContext.Users.Add(user);
              dbContext.SaveChanges();
              HttpContext.Session.SetInt32("userId", user.UserId);
                
              return RedirectToAction("Dashboard");

          }
          return View("registration");
        } 

        [HttpPost("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
          if(ModelState.IsValid)
          {
            User userInDb = dbContext.Users.FirstOrDefault(u=> u.Email == userSubmission.LoginEmail);
            if(userInDb == null)
              {
                ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                return View("Index");
              }
                //These two lines will compare our hashed passwords.
                var hash = new PasswordHasher<LoginUser>();
                var result = hash.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                //Result will either be 0 or 1.
                if(result == 0)
                {
                  ModelState.AddModelError("LoginPassword", "Invalid Email/Password");
                  return View("Index");
                } 
                HttpContext.Session.SetInt32("userId", userInDb.UserId);
                return RedirectToAction("Dashboard");
          }
          return View("Index");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
          HttpContext.Session.Clear();
          return RedirectToAction("Index");
        }

        ///////////////////////////////////
        //    WEDDING 
        //////////////////////////////////

        [HttpGet("weddings")]
        public IActionResult Dashboard()
        {
          User current = GetUser();
          if (current == null)
          {
            return RedirectToAction("Index");
          }
          ViewBag.User = current;
          List<Wedding> weddings = dbContext.Weddings
                                            .Include(w => w.Planner)
                                            .Include(w => w.Attendees)
                                            .Where(w => w.Date > DateTime.Now)
                                            .ToList();
          return View(weddings);
        }


        [HttpGet("new")]
        public IActionResult New()
        {
          User current = GetUser();
          if (current == null)
          {
            return RedirectToAction("Index");
          }
          return View();
        }
        ///////////////////////////////////
        //    CREATE NEW WEDDING
        //////////////////////////////////
        [HttpPost("create")]
        public IActionResult Create(Wedding newWedding)
        {
          User current = GetUser();
          if (current == null)
          {
            return RedirectToAction("Index");
          }
          if(ModelState.IsValid)
          {
            newWedding.UserId = current.UserId;
            dbContext.Add(newWedding);
            dbContext.SaveChanges();
            Wedding addedWedding = dbContext.Weddings
                                            .OrderByDescending(w => w.CreatedAt)
                                            .FirstOrDefault();
            Association rsvp = new Association();
            rsvp.UserId = current.UserId;
            rsvp.WeddingId = addedWedding.WeddingId;
            dbContext.Add(rsvp);
            dbContext.SaveChanges();
            return Redirect ($"/weddings/{addedWedding.WeddingId}");
          }
          return View("New");
        }

        ///////////////////////////////////
        //    WEDDING_ID
        //////////////////////////////////

        [HttpGet("weddings/{weddingId}")]
        public IActionResult Details(int weddingId)
        {
          User current = GetUser();
          if (current == null)
          {
            return RedirectToAction("Index");
          }
          Wedding wedding = dbContext.Weddings
                                    .Include(w => w.Attendees)
                                    .ThenInclude(a=>a.Attendee)
                                    .Include(w => w.Planner)
                                    .FirstOrDefault(w => w.WeddingId == weddingId);
          ViewBag.User = current;
          return View(wedding);
        }

        ///////////////////////////////////
        //    WEDDING_RSVP
        //////////////////////////////////

        [HttpGet("weddings/{weddingId}/rsvp")]
        public IActionResult RSVP(int weddingId)
        {
          User current = GetUser();
          if (current == null)
          {
            return RedirectToAction("Index");
          }
          Association rsvp = new Association();
          rsvp.WeddingId = weddingId;
          rsvp.UserId = current.UserId;
          dbContext.Add(rsvp);
          dbContext.SaveChanges();
          return RedirectToAction("Dashboard");
        }
        ///////////////////////////////////
        //    WEDDING VIDMOROZ
        //////////////////////////////////

        [HttpGet("weddings/{weddingId}/leave")]
        public IActionResult Leave(int weddingId)
        {
          User current = GetUser();
          if (current == null)
          {
            return RedirectToAction("Index");
          }
            Association leaving =  dbContext.Associations.FirstOrDefault( a => a.WeddingId == weddingId && a.UserId == current.UserId);
            dbContext.Associations.Remove(leaving);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        ///////////////////////////////////
        //    WEDDING REMOVE
        //////////////////////////////////

        [HttpGet("weddings/{weddingId}/delete")]
        public IActionResult Delete(int weddingId)
        {
          User current = GetUser();
          if (current == null)
          {
            return RedirectToAction("Index");
          }
            Wedding toDelete =  dbContext.Weddings.FirstOrDefault( w => w.WeddingId == weddingId);
            dbContext.Weddings.Remove(toDelete);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}