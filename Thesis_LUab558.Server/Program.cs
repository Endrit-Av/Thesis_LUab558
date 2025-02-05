using Microsoft.EntityFrameworkCore;
using Thesis_LUab558.Server.Apps;
using Thesis_LUab558.Server.Data;
using Thesis_LUab558.Server.Mappings;
using Thesis_LUab558.Server.Services;

bool runDbSetup = false; // Auf false setzen nach einmaligen Ausführen.

if (runDbSetup)
{
    Console.WriteLine("Starte einmalige Datenbank-Initialisierung...");
    DBApp.InitializeDatabase();
    Console.WriteLine("Datenbank-Initialisierung abgeschlossen.");
}

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

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
    app.UseSwagger(); 
    app.UseSwaggerUI(); 
}

app.UseCors("FrontendPolicy");

app.UseResponseCaching();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
