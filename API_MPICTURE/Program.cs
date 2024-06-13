using API_MPICTURE.Models.Services;
using API_MPICTURE.Data;
using API_MPICTURE.Models.Interfaces;
using API_MPICTURE.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Add services to the container.
var _logger = new LoggerConfiguration()
 .WriteTo.Console()// ghi ra console
 .WriteTo.File("Logs/Image_log.txt", rollingInterval: RollingInterval.Minute) //ghi ra file lưu trong thư mục Logs
 .MinimumLevel.Information()
 .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(_logger);

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Image API",
        Version = "v1"
    });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


// Đăng ký DbContext cho AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký DbContext cho ImageAuthDbContext
builder.Services.AddDbContext<ImageAuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ImageAuthConnection")));

// Đăng ký repository
builder.Services.AddScoped<IImageRepository, SQLImageRepository>();
builder.Services.AddScoped<ICategoryRepository, SQLCategoryRepository>();
builder.Services.AddScoped<ITagRepository, SQLTagRepository>();

builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IUDonwImageRepository,LocalUDownImageRepository>();

builder.Services.AddIdentityCore<IdentityUser>()
 .AddRoles<IdentityRole>()
 .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Image")
 .AddEntityFrameworkStores<ImageAuthDbContext>()
 .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireDigit = false;// Yêu cầu về password chứa ký số không?
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 1;
});
// Cấu hình Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
