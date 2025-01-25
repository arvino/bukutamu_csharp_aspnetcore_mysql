namespace BukuTamuApp.Models
{
    public class BukuTamu
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public required string Messages { get; set; }
        public string? Gambar { get; set; }  // Optional
        public DateTime Timestamp { get; set; }
        public required Member Member { get; set; }
    }
} 