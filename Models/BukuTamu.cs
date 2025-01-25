namespace BukuTamuApp.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BukuTamu
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        
        [Required(ErrorMessage = "Pesan harus diisi")]
        public string Messages { get; set; } = string.Empty;
        
        public string? Gambar { get; set; }  // Optional
        public DateTime Timestamp { get; set; }
        public required Member Member { get; set; }

        public BukuTamu()
        {
            Timestamp = DateTime.Now;  // Set default value
        }
    }
} 