using System.Security.Claims;
using IdentityModel;
using IdentityService.Models;
using IdentityService.Pages.Account.Register;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Register;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;

    [BindProperty]
    public RegisterViewModel Input { get; set; }

    [BindProperty]
    public bool RegistrationSuccess { get; set; }

    public Index(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public IActionResult OnGet(string returnUrl)
    {
        Input = new RegisterViewModel
        {
            ReturnUrl = returnUrl
        };

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        if (this.Input.Button != "register")
        {
            return this.Redirect("~/");
        }

        // verify things are valid,
        if (this.ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = this.Input.Username,
                Email = this.Input.Email,
                EmailConfirmed = true
            };

            var result = await this.userManager.CreateAsync(user, this.Input.Password);

            if (result.Succeeded)
            {
                await this.userManager.AddClaimsAsync(user, new Claim[]{
                     new Claim(JwtClaimTypes.Name, this.Input.FullName)
                });

                this.RegistrationSuccess = true;
            }
        }

        return this.Page();
    }
}
