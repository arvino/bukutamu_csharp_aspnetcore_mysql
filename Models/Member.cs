using System.ComponentModel.DataAnnotations;

namespace BukuTamuApp.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama harus diisi")]
        [StringLength(100, ErrorMessage = "Nama maksimal 100 karakter")]
        public string Nama { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Format nomor telepon tidak valid")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Email harus diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        [StringLength(100, ErrorMessage = "Email maksimal 100 karakter")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password harus diisi")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password minimal 6 karakter")]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = "member";
        
        public ICollection<BukuTamu> BukuTamus { get; set; } = new List<BukuTamu>();
    }
} 