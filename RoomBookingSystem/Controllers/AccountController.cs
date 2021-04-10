using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomBookingSystem.Models;
using RoomBookingSystem.Services;

namespace RoomBookingSystem.Controllers
{

    public class AccountController : Controller
    {
        readonly RoomBookingContext _bokingDBContext;

        public AccountController(RoomBookingContext bokingDBContext)
        {
            _bokingDBContext = bokingDBContext;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel model)
        {
            var User = _bokingDBContext.Users.Where(u => u.Username == model.Username.Trim() && u.Password == model.Password && u.Isactive == true).FirstOrDefault();
            if (User != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,User.Username),
                        new Claim(ClaimTypes.Upn,User.UserId.ToString())
                    };

                var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    IsPersistent = false,
                    IssuedUtc = DateTime.Now
                };

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
                return RedirectToAction("Index", "Rooms");
            }

            ViewBag.Error = "Username or Password doesn't match";
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Create()
        {
            SelectList states = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "-- SELECT CITY --", Value = ""},
                new SelectListItem { Text = "New York City", Value = "New York City"},
                new SelectListItem { Text = "Toronto", Value = "Toronto"},
            }, "Value", "Text");
            SelectList countries = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Text = "-- SELECT COUNTRY --", Value = ""},
                new SelectListItem { Text = "USA", Value = "United States"},
                new SelectListItem { Text = " Canada", Value = "Canada"},
            }, "Value", "Text");
            return View(new UserModel() { States = states, Countries = countries });
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserModel model)
        {
            try
            {
                model.Isactive = true;
                _bokingDBContext.Add(model);
                _bokingDBContext.SaveChanges();

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,model.Username),
                        new Claim(ClaimTypes.Upn,model.UserId.ToString())
                    };

                var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = false,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                    IsPersistent = false,
                    IssuedUtc = DateTime.Now
                };

                await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error", "Rooms", new { ErrorMessage = ex.Message });
            }

            return RedirectToAction("Index", "Rooms");
        }
    }
}