using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Function;


var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDB")));

// Add services to the container.
builder.Services.AddScoped<UserRepository> ();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var tokenKey = Encoding.ASCII.GetBytes("ntqbqhrychczumzisfmojgjtpvpsfgwm");
var secretKey = new SymmetricSecurityKey(tokenKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", jwtBearerOptions =>
{
    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
        ValidateIssuer = true,
        ValidIssuer = "https://localhost:7145/swagger",
        ValidateAudience = true,
        ValidAudience = "api",
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseJwtTokenMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
