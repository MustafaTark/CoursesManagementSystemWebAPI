using CoursesManagementSystem;
using CoursesManagementSystem.Auth;
using CoursesManagementSystem.Data;
using CoursesManagementSystem.Models;
using CoursesManagementSystem.Repo;
using CoursesManagementSystem.Repositeries;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IServiceRepositery<Student>, StudentRepositery>();
builder.Services.AddScoped<IServiceRepositery<Instructor>, InstructorRepositery>();
builder.Services.AddScoped<IServiceRepositery<Course>, CourseRepositery>();
builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
var builderIdentity = builder.Services.AddIdentityCore<User>(o => {
    o.Password.RequireDigit = true;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 10;
    o.User.RequireUniqueEmail = true;
   
});
builderIdentity = new IdentityBuilder(
    builderIdentity.UserType,
    typeof(IdentityRole), builder.Services);
builderIdentity.AddEntityFrameworkStores<ApplicationDb>().AddDefaultTokenProviders();
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDb>(options => {
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Environment.GetEnvironmentVariable("SECRET");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters =
       new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
           ValidAudience = jwtSettings.GetSection("validAudience").Value,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
       };
}).AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"), "jwtBearerScheme2");

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();
//var setting = new Newtonsoft.Json.JsonSerializer(); ;
//setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
//setting.Formatting = (Newtonsoft.Json.Formatting)Formatting.Indented;
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
