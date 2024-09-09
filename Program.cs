using BlogAPI.Services;
using BlogAPI.utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {

        Title = "Authdemo",
        Version = "v1"

    });
    option.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme()
    {

        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "please enter a token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
{
    {
        new OpenApiSecurityScheme
        {

            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"

            }

        }, []

    }
});
});



builder.Services.AddScoped<IUserManger,UserManger>();
string ConnectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AppDbContext>(options  => options.UseSqlServer(ConnectionString));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
