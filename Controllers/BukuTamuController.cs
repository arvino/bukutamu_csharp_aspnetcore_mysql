using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BukuTamuApp.Models;
using BukuTamuApp.Data;

namespace BukuTamuApp.Controllers
{
    public class BukuTamuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BukuTamuController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var bukuTamus = await _context.BukuTamus
                .Include(b => b.Member)
                .OrderByDescending(b => b.Timestamp)
                .ToListAsync();
            return View(bukuTamus);
        }

        [Authorize(Policy = "MemberPolicy")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "MemberPolicy")]
        public async Task<IActionResult> Create(BukuTamu bukuTamu, IFormFile gambar)
        {
            if (ModelState.IsValid)
            {
                if (gambar != null)
                {
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(gambar.FileName);
                    var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await gambar.CopyToAsync(stream);
                    }

                    bukuTamu.Gambar = fileName;
                }

                bukuTamu.MemberId = int.Parse(User.FindFirst("MemberId").Value);
                bukuTamu.Timestamp = DateTime.Now;

                _context.BukuTamus.Add(bukuTamu);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(bukuTamu);
        }

        public async Task<IActionResult> Details(int id)
        {
            var bukuTamu = await _context.BukuTamus
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bukuTamu == null)
            {
                return NotFound();
            }

            return View(bukuTamu);
        }
    }
} 