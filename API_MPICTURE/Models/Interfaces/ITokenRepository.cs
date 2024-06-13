using Microsoft.AspNetCore.Identity;
namespace API_MPICTURE.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
