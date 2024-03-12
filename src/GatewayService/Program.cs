using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServiceUrl"];

        // TLS is handled at the Router/K(8)s, not here.
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters.ValidateAudience = false;

        // this allows us to use User.Identity.Name in the Controller to
        // get the "username" claim value.
        options.TokenValidationParameters.NameClaimType = "username";
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("customPolicy", b =>
    {
        b.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(builder!.Configuration["ClientApp"]!);
    });
});

var app = builder.Build();

app.UseCors();
app.MapReverseProxy();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
