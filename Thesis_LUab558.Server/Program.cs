using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.Mappings;
using Thesis_LUab558.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddScoped<TestService>();      // korrekt in den Dependency Injection (DI)-Container von ASP.NET Core registriert
builder.Services.AddScoped<MainpageService>();  // korrekt in den Dependency Injection (DI)-Container von ASP.NET Core registriert

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:54495", "https://127.0.0.1:54495") // Oder allgemein: .AllowAnyOrigin() Erlaubt alle Ursprünge
              .WithMethods("GET", "POST") //.AllowAnyMethod() Erlaubt alle HTTP-Methoden (GET, POST, etc.)
              .WithHeaders("Content-Type", "Authorization");  //.AllowAnyHeader(); Erlaubt alle Header
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer(); // Fügt Swagger/OpenAPI-Dokumentation hinzu
builder.Services.AddSwaggerGen(c =>         // Fügt Swagger hinzu
{
    c.EnableAnnotations();
});      

builder.Services.AddDbContext<NasDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseDeveloperExceptionPage(); // Für bessere Fehlermeldungen
    app.UseSwagger(); // Aktiviert Swagger in der Entwicklungsumgebung
    app.UseSwaggerUI(); // Aktiviert die Swagger-Benutzeroberfläche
}

app.UseCors("FrontendPolicy"); // Spezifische CORS-Policy anwenden

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles(); // Aktiviert die Bereitstellung von statischen Dateien aus wwwroot

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
