namespace spotify_rating.Data.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public string? UpdatedBy { get; set; }
    public bool Active { get; set; } = true;
    public DateTime? DeactivatedAtUtc { get; set; }
    public string? DeactivatedBy { get; set; }
}