namespace BukuTamuApp.Models.DTOs
{
    public class BukuTamuDTO
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public required string Messages { get; set; }
        public string? Gambar { get; set; }
        public DateTime Timestamp { get; set; }
        public required string MemberNama { get; set; }
    }

    public class CreateBukuTamuDTO
    {
        public required string Messages { get; set; }
        public IFormFile? Gambar { get; set; }
    }

    public class UpdateBukuTamuDTO
    {
        public required string Messages { get; set; }
        public IFormFile? Gambar { get; set; }
    }
} 