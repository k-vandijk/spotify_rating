namespace spotify_rating.Data.Entities;

public class TrafficLog
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? UserId { get; set; }
    public string Path { get; set; } = null;
    public string Method { get; set; } = null;
    public string? IPAddress { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}