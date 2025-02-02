using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.Mappings;
using Thesis_LUab558.Server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<CartService>();

builder.Services.AddResponseCaching();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<NasDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("https://localhost:54495", "https://127.0.0.1:54495")
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.EnableAnnotations();
});      

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

app.UseResponseCaching(); //Aktiviert Caching --> nach Cors implementieren

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles(); // Aktiviert die Bereitstellung von statischen Dateien aus wwwroot

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
