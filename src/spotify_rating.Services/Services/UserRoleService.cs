using spotify_rating.Data;
using spotify_rating.Data.Entities;
using spotify_rating.Data.Enums;

namespace spotify_rating.Services.Services;

public interface IUserRoleService
{
    Role GetRoleForUser(string spotifyUserId);
}

public class UserRoleService : IUserRoleService
{
    private readonly DataContext _context;

    public UserRoleService(DataContext context)
    {
        _context = context;
    }

    public Role GetRoleForUser(string spotifyUserId)
    {
        var role = _context.UserRoles.FirstOrDefault(r => r.UserId == spotifyUserId);

        if (role == null)
        {
            role = new UserRole { UserId = spotifyUserId, CreatedBy = spotifyUserId };
            _context.UserRoles.Add(role);
            _context.SaveChanges();
        }

        return role.Role;
    }
}
