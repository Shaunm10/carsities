using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Services;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> userManager;
    public CustomProfileService(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        // subject should be the user's Id.
        var user = await this.userManager.GetUserAsync(context.Subject);
        var existingClaims = await this.userManager.GetClaimsAsync(user);

        // create a list of custom claims
        var claims = new List<Claim>
        {
            new Claim("username", user.UserName)
        };

        // add these custom claims to the context being sent back.
        context.IssuedClaims.AddRange(claims);
        context.IssuedClaims.Add(existingClaims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name));
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return Task.CompletedTask;
    }

}
