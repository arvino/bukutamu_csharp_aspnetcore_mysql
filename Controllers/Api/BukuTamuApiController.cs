using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BukuTamuApp.Models;
using BukuTamuApp.Models.DTOs;
using BukuTamuApp.Data;

namespace BukuTamuApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class BukuTamuApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BukuTamuApiController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: api/BukuTamuApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BukuTamuDTO>>> GetBukuTamus()
        {
            var bukuTamus = await _context.BukuTamus
                .Include(b => b.Member)
                .OrderByDescending(b => b.Timestamp)
                .Select(b => new BukuTamuDTO
                {
                    Id = b.Id,
                    MemberId = b.MemberId,
                    Messages = b.Messages,
                    Gambar = b.Gambar,
                    Timestamp = b.Timestamp,
                    MemberNama = b.Member.Nama
                })
                .ToListAsync();

            return bukuTamus;
        }

        // GET: api/BukuTamuApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BukuTamuDTO>> GetBukuTamu(int id)
        {
            var bukuTamu = await _context.BukuTamus
                .Include(b => b.Member)
                .Where(b => b.Id == id)
                .Select(b => new BukuTamuDTO
                {
                    Id = b.Id,
                    MemberId = b.MemberId,
                    Messages = b.Messages,
                    Gambar = b.Gambar,
                    Timestamp = b.Timestamp,
                    MemberNama = b.Member.Nama
                })
                .FirstOrDefaultAsync();

            if (bukuTamu == null)
            {
                return NotFound();
            }

            return bukuTamu;
        }

        // POST: api/BukuTamuApi
        [Authorize(Policy = "MemberPolicy")]
        [HttpPost]
        public async Task<ActionResult<BukuTamuDTO>> CreateBukuTamu([FromForm] CreateBukuTamuDTO dto)
        {
            var memberId = int.Parse(User.FindFirst("MemberId").Value);

            var bukuTamu = new BukuTamu
            {
                MemberId = memberId,
                Messages = dto.Messages,
                Timestamp = DateTime.Now
            };

            if (dto.Gambar != null)
            {
                var fileName = Path.GetRandomFileName() + Path.GetExtension(dto.Gambar.FileName);
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Gambar.CopyToAsync(stream);
                }

                bukuTamu.Gambar = fileName;
            }

            _context.BukuTamus.Add(bukuTamu);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBukuTamu), new { id = bukuTamu.Id }, bukuTamu);
        }

        // PUT: api/BukuTamuApi/5
        [Authorize(Policy = "MemberPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBukuTamu(int id, [FromForm] UpdateBukuTamuDTO dto)
        {
            var bukuTamu = await _context.BukuTamus.FindAsync(id);
            if (bukuTamu == null)
            {
                return NotFound();
            }

            var memberId = int.Parse(User.FindFirst("MemberId").Value);
            if (!User.IsInRole("admin") && bukuTamu.MemberId != memberId)
            {
                return Forbid();
            }

            bukuTamu.Messages = dto.Messages;

            if (dto.Gambar != null)
            {
                // Hapus gambar lama jika ada
                if (!string.IsNullOrEmpty(bukuTamu.Gambar))
                {
                    var oldFilePath = Path.Combine(_environment.WebRootPath, "uploads", bukuTamu.Gambar);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Upload gambar baru
                var fileName = Path.GetRandomFileName() + Path.GetExtension(dto.Gambar.FileName);
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Gambar.CopyToAsync(stream);
                }

                bukuTamu.Gambar = fileName;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BukuTamuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/BukuTamuApi/5
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBukuTamu(int id)
        {
            var bukuTamu = await _context.BukuTamus.FindAsync(id);
            if (bukuTamu == null)
            {
                return NotFound();
            }

            // Hapus file gambar jika ada
            if (!string.IsNullOrEmpty(bukuTamu.Gambar))
            {
                var filePath = Path.Combine(_environment.WebRootPath, "uploads", bukuTamu.Gambar);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.BukuTamus.Remove(bukuTamu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BukuTamuExists(int id)
        {
            return _context.BukuTamus.Any(e => e.Id == id);
        }
    }
} 