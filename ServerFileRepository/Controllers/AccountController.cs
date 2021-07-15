using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerFileRepository.Contexts;
using ServerFileRepository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ServerFileRepository.Contexts.Tables;

namespace ServerFileRepository.Controllers
{
    public class AccountController : Controller
    {
        private UserContext db;

        public AccountController(UserContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                //Убрать прямую проверку по паролю
                var passWithSalt = model.Password + model.Login;
                var passHash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(passWithSalt)));
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login && u.Password == passHash);
                if (user != null)
                {
                    await Authenticate(model.Login);

                    return RedirectToAction("Index", "FileSystem");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {                
                User user = await db.Users.FirstOrDefaultAsync(u => u.Login == model.Login);
                if (user == null)
                {
                    var passWithSalt = model.Password + model.Login;
                    var passHash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(passWithSalt)));
                    db.Users.Add(new User { Login = model.Login, Password = passHash });
                    await db.SaveChangesAsync();
                    await Authenticate(model.Login);
                    return RedirectToAction("Index", "FileSystem");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }


    }
}
