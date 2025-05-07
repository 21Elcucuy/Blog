using System.Text;
using Backend.Application.Shared.Configurations;
using Backend.Application.UseCases;
using Backend.Domain.Interfaces;
using Backend.Domain.Models;
using Backend.Infrastructure.Persistence.Database;
using Backend.Infrastructure.Persistence.Repository;
using Backend.Infrastructure.Services;
using Blog.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

builder.Services.AddDbContext<BlogContext>(options =>
    options.UseInMemoryDatabase("Blog"));
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseInMemoryDatabase("AuthBlog"));
builder.Services.AddScoped<PostServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IPostRepository, SQLPostRepository>();
builder.Services.AddScoped<IFollowServices, FollowServices>();
builder.Services.AddScoped<FollowServicesOutPut>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(op =>
    {
        op.RequireHttpsMetadata = false;
        op.SaveToken = false;
        op.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))

        };

    }
);
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