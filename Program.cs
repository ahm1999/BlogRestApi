using BlogAPI.Services;
using BlogAPI.utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ItokenService,TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme )
    .AddJwtBearer(options =>{

        options.TokenValidationParameters = new TokenValidationParameters(){
            ValidateIssuerSigningKey = true
            ,IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSecret")))
            ,ValidateIssuer = false
            ,ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<IUserManger,UserManger>();
string ConnectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AppDbContext>(options  => options.UseSqlServer(ConnectionString));
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
