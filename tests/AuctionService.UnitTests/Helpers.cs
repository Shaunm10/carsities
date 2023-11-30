using System.Security.Claims;

namespace AuctionService.UnitTests;

public class Helpers
{
    public static ClaimsPrincipal GetClaimsPrincipal(string userName = null)
    {
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName ?? "test") };
        var identity = new ClaimsIdentity(claims, "testing");
        return new ClaimsPrincipal(identity);
    }
}
