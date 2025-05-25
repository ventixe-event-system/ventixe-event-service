// Program.cs - Event Service
using Microsoft.EntityFrameworkCore;
using ventixe_event_service.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseSqlite("Data Source=events.db"));

// CORS för att tillåta frontend att prata med backend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // React app
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

// Skapa databasen automatiskt
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EventDbContext>();
    context.Database.EnsureCreated();
}

Console.WriteLine("?? Event Service API startar...");
Console.WriteLine("?? Swagger UI: https://localhost:7002/swagger");
Console.WriteLine("?? API Base URL: http://localhost:5272/api");

app.Run();