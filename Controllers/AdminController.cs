using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BukuTamuApp.Models;
using BukuTamuApp.Data;

namespace BukuTamuApp.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var members = await _context.Members.Where(m => m.Role == "member").ToListAsync();
            return View(members);
        }

        public async Task<IActionResult> BukuTamuList()
        {
            var bukuTamus = await _context.BukuTamus
                .Include(b => b.Member)
                .OrderByDescending(b => b.Timestamp)
                .ToListAsync();
            return View(bukuTamus);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBukuTamu(int id)
        {
            var bukuTamu = await _context.BukuTamus.FindAsync(id);
            if (bukuTamu != null)
            {
                if (!string.IsNullOrEmpty(bukuTamu.Gambar))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", bukuTamu.Gambar);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                _context.BukuTamus.Remove(bukuTamu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(BukuTamuList));
        }
    }
} 