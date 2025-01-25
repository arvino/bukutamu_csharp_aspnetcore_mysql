public class BukuTamu
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public string Messages { get; set; }
    public string Gambar { get; set; }
    public DateTime Timestamp { get; set; }
    public virtual Member Member { get; set; }
} 