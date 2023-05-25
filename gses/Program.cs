using Gses.Infrastructure;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRazorPages();
builder.Services.AddDependencies();
builder.Services.AddHttpClient();

var appTitle = "GSES2 BTC application";
builder.Services.AddSwaggerGen(o => {
	o.SwaggerDoc("Gses", new OpenApiInfo {
		Title = appTitle,
		Version = "1.0.0"
	});

	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	o.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

const string basePath = "/api";
app.UsePathBase(basePath);

// Configure the HTTP request pipeline.
app.UseSwagger(c => {
	c.RouteTemplate = "/swagger/{documentname}/swagger.json";
	c.PreSerializeFilters.Add((swagger, httpReq) => {
		swagger.Servers = new List<OpenApiServer> { new() { Url = $"http://{httpReq.Host.Value}{basePath}" } };
	});
});

app.UseSwaggerUI(c => {
	c.SwaggerEndpoint("/swagger/Gses/swagger.json", appTitle);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
