using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BukuTamuApp.Models;
using BukuTamuApp.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace BukuTamuApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hashedPassword = HashPassword(password);
            var user = _context.Members.FirstOrDefault(m => m.Email == email && m.Password == hashedPassword);

            if (user == null)
            {
                ModelState.AddModelError("", "Email atau password salah");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("MemberId", user.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (user.Role == "admin")
                return RedirectToAction("Index", "Admin");
            return RedirectToAction("Index", "BukuTamu");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Member model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                // Cek email sudah terdaftar
                if (await _context.Members.AnyAsync(m => m.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email sudah terdaftar");
                    return View(model);
                }

                // Set default values
                model.Password = HashPassword(model.Password);
                model.Role = "member";
                model.BukuTamus = new List<BukuTamu>();

                _context.Members.Add(model);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Registrasi berhasil! Silakan login.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Terjadi kesalahan: {ex.Message}");
                return View(model);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "BukuTamu");
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
} 