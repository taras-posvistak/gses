using Gses.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(o => {
	o.SwaggerDoc("Gses", new OpenApiInfo {
		Title = "GSES2 BTC application",
		Version = "1.0.0"
	});

	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	o.IncludeXmlComments(xmlPath);
});

builder.Services.AddDependencies();
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger(c => {
		c.RouteTemplate = "/swagger/{documentname}/swagger.json";
		c.PreSerializeFilters.Add((swagger, httpReq) => {
			swagger.Servers = new List<OpenApiServer> { new() { Url = $"http://{httpReq.Host.Value}" } };
		});
	});

	app.UseSwaggerUI(c => {
		c.SwaggerEndpoint("/swagger/Gses/swagger.json", "Gses");
	});
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
