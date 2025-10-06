using MainProject;
using MainProject.Authrization;
using MainProject.Data;
using MainProject.Filters;
using MainProject.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Runtime;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("config.json");
// Add services to the container.

builder.Services.Configure<AttachmentOptions>(builder.Configuration.GetSection("Attachments"));

builder.Services.AddLogging(cfg =>
{
    cfg.AddDebug();
});



// way one 
//var attachmentOptions = builder.Configuration.GetSection("Attachments").Get<AttachmentOptions>();
//builder.Services.AddSingleton(attachmentOptions);

// way two 
//var attachmentOptions = new AttachmentOptions();
//builder.Configuration.GetSection("Attachments").Bind(attachmentOptions);
//builder.Services.AddSingleton(attachmentOptions);


builder.Services.AddControllers(options =>
{
    options.Filters.Add<LogActivityFilter>();   
    options.Filters.Add<LogSensitiveActionAttribute>();
    options.Filters.Add<PermissionBasedAuthorizationFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// integrated security=true -> windows Authentication
builder.Services.AddDbContext<ApplicationDbContext>(cfg => cfg.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));
/* sql server Authentication
builder.Services.AddDbContext<ApplicationDbContext>(builder => builder.UseSqlServer("server=.;database=Products;user id=sa;password=sa123456"));
*/

/* check if System.Globalization.Invariant = true / you can make it false from MainProject.csproj
Console.WriteLine(AppContext.GetData("System.Globalization.Invariant"));
*/
var JwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddSingleton(JwtOptions);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperUsersOnly", builder =>
    {
        builder.RequireClaim(ClaimTypes.Role, "SuperUser");     
        builder.RequireRole("SuperUser");
    });
    options.AddPolicy("EmployeesOnly", builder =>
    {
        builder.RequireClaim("UserType", "Employee");
    });
});

builder.Services.AddAuthentication()
    // JwtBearerDefaults.AuthenticationScheme or "Bearer"
    //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    //{

    //});
    .AddJwtBearer("Bearer", options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = JwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Signingkey))
        };
    });
    //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ProfilingMiddleware>();

app.MapControllers();
// StaticFile Don't interact with the actionFilter cus there is no request and response 
app.UseStaticFiles();

app.Run();
