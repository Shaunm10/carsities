using BiddingService.BackgroundServices;
using BiddingService.BusinessServices;
using BiddingService.Consumers;
using BiddingService.GrpcServices;
using MassTransit;
using MessageContracts.Auction;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMQ:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMQ:Password", "guest"));
        });

        cfg.ConfigureEndpoints(context);
    });
});

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


builder.Services.AddScoped<IAuctionService, BiddingService.BusinessServices.AuctionService>();
builder.Services.AddScoped<IBidService, BidService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<CheckAuctionFinished>();

builder.Services.AddScoped<GrpcAuctionClient>();
var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseAuthorization();

app.MapControllers();

// initializes a new Mongo database.
await DB.InitAsync("BidDb", MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("BidDbConnection")));

app.Run();
