using MainProject;
using MainProject.Data;
using MainProject.Filters;
using MainProject.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Runtime;

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
builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

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
