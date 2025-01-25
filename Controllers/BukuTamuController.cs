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

        [Authorize(Roles = "member,admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "member,admin")]
        public async Task<IActionResult> Create(BukuTamu bukuTamu, IFormFile? gambar)
        {
            var memberId = int.Parse(User.FindFirst("MemberId")?.Value ?? "0");
            var member = await _context.Members.FindAsync(memberId);
            if (member == null)
            {
                return BadRequest("Member tidak ditemukan");
            }

            try
            {
                bukuTamu.Member = member;
                bukuTamu.MemberId = memberId;
                bukuTamu.Timestamp = DateTime.Now;

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

                _context.BukuTamus.Add(bukuTamu);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error saat menyimpan: " + ex.Message);
                return View(bukuTamu);
            }
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