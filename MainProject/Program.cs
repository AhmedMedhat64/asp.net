using MainProject.Data;
using MainProject.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Runtime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// integrated security=true -> windows Authentication
builder.Services.AddDbContext<ApplicationDbContext>(builder => builder.UseSqlServer("server=.;database=Products;integrated security=true;trust server certificate=true"));
/* sql server Authentication
builder.Services.AddDbContext<ApplicationDbContext>(builder => builder.UseSqlServer("server=.;database=Products;user id=sa;password=sa123456"));
*/

/* check if System.Globalization.Invariant = true / you can make it false from MainProject.csproj
Console.WriteLine(AppContext.GetData("System.Globalization.Invariant"));
*/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ProfilingMiddleware>();

app.MapControllers();

app.Run();
