using System.Security.Claims;
using System.Text;
using Application.AdminServices.Interface;
using Application.AdminServices.Services;
using Application.Authentication.Interface;
using Application.CommentServices.Extenstion;
using Application.CommentServices.Interface;
using Application.CommentServices.Services;
using Application.EmailServices.Interface;
using Application.EmailServices.Services;
using Application.LikeServices.Extinstion;
using Application.LikeServices.Interface;
using Application.LikeServices.Services;
using Application.PostSev;
using Application.PostSer.Services;
using Application.PostSev.Extenstion;
using Application.PostSev.Interface;
using Domain.Entity.Identity;
using Domain.Interface;
using Helper.EmailSettings;
using Infrastructure.Persistence.DbContexts;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.Seed;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project.JWT;
using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Host.UseSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
    opt.TokenLifespan = TimeSpan.FromHours(2);
});
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite("Data Source= ./Data/AuthDb.db"));
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlite("Data Source= ./Data/BlogDb.db"));
builder.Services.AddScoped<IEmailServices, EmailServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IPostServices, PostServices>();
builder.Services.AddScoped<IPostRepository,SqlPostRepository>();
builder.Services.AddScoped<PostMapping>();
builder.Services.AddScoped<IConfrimEmailServices , ConfrimEmailServices>();
builder.Services.AddScoped<IResetPasswordServices, ResetPasswordSevices>();
builder.Services.AddScoped<ICommentRepository, SqlCommentRepository>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<ICommentMapping, CommentMapping>();
builder.Services.AddScoped<ILikeRepository, SqlLikeRepository>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<ILikeMapping, LikeMapping>();
builder.Services.AddScoped<IAdminService, AdminService>();
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
        op.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))

        };
        op.Events = new JwtBearerEvents()
        {
            OnTokenValidated = async context =>
            {
                var userManager =
                    context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
                var user = await userManager.GetUserAsync(context.Principal);
                if (user == null || user.LockoutEnd > DateTimeOffset.Now)
                {
                    context.Fail("User is banned or dosent not exist ");
                    return;
                }

                var securityStamp = context.Principal.FindFirstValue("AspNet.Identity.SecurityStamp");
                var currentStmp = await userManager.GetSecurityStampAsync(user);
                if (securityStamp != currentStmp)
                {
                    context.Fail("SecurityStamp mismatch , Token Invalidate");
                }
            }
        };

    }
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") 
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await IdentitySeed.SeedRolesAsync(roleManager);
    await  IdentitySeed.SeedUsersAsync(userManager);
  
    
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();


app.Run();

