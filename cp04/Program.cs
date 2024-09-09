using CP04.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    x => x.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Checkpoint 4 - RM552314",
        Version = "v1",
        Description = "API para procurar países",
        Contact = new OpenApiContact() { Email = "yaslopesyweb@gmail.com", Name = "Yasmin Araujo Santos Lopes" }
    }
));

builder.Services.AddHttpClient();


builder.Services.AddScoped<ICountryService, CountryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
