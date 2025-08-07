using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScrumStandUpTrackerProject.DataLayer;
using ScrumStandUpTrackerProject.Mappers;
using ScrumStandUpTrackerProject.Repositories;
using ScrumStandUpTrackerProject.Services;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt",
                  rollingInterval: RollingInterval.Day,  // creates new file daily
                  outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();
// Add DbContext with SQL Server provider


if (builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseInMemoryDatabase("ScrumStandUpTrackerInMemoryDb"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}


// Register repository and service
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IDailyStatusRepository, DailyStatusRepository>();
builder.Services.AddScoped<IDailyStatusService, DailyStatusService>();

builder.Services.AddAutoMapper(typeof(EntityMapper));
// Add controllers and Swaggerx`
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Authoriztion
builder.Services.AddAuthorization();

// CORS policy name
var MyAllowReactOrigins = "_myAllowReactOrigins";

// Configure CORS to allow React app origin
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowReactOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")  // React app URL
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

// JWT Authentication Setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
});


var app = builder.Build();

app.UseMiddleware<ScrumStandUpTrackerProject.Exceptions.ExceptionMiddleware>();



// Configure middleware
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing"))
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();   
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS before authentication and authorization middleware
app.UseCors(MyAllowReactOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();