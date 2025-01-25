namespace BukuTamuApp.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public virtual ICollection<BukuTamu> BukuTamus { get; set; }
    }
} 