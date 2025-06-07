using spotify_rating.Data.Enums;

namespace spotify_rating.Data.Entities;

public class UserRole : BaseEntity
{
    public string UserId { get; set; }
    public Role Role { get; set; } = Role.User;
}