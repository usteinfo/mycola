using ApiServerSample;
using ApiServerSample.BusinessCommand;
using MyCloa.Common.Api;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//1¡¢×¢²á·þÎñ
builder.Services.AddApiHelp(new ApiHelpOption()
{
    ResolveType = typeof(ResolveService),
    ServerName = "ServerName"
});

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

//2¡¢É¨ÃèÃüÁî
builder.Services.ScanCommand(new List<Assembly> { typeof(HelloCommand).Assembly });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
