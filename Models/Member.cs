namespace BukuTamuApp.Models
{
    public class Member
    {
        public int Id { get; set; }
        public required string Nama { get; set; }
        public string? Phone { get; set; }  // Optional
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public ICollection<BukuTamu> BukuTamus { get; set; } = new List<BukuTamu>();
    }
} 