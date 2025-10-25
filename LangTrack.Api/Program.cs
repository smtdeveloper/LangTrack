using LangTrack.Application;
using LangTrack.Infrastructure;
using LangTrack.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "LangTrack API", 
        Version = "v1",
        Description = "Günlük İngilizce Öğrenme API'si - Kelime takibi ve çalışma kayıtları"
    });
});

// Add application services
builder.Services.AddApplication();

// Add infrastructure services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=langtrack.db";
builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LangTrack API v1");
        c.RoutePrefix = string.Empty; // Swagger UI'ı root'ta göster
    });
}

app.UseHttpsRedirection();
app.UseRouting();

// Global exception handling
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();
app.MapControllers();

// Ensure database is created and seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<LangTrack.Infrastructure.Data.LangTrackDbContext>();
    context.Database.EnsureCreated();
    
    // Seed initial data
    await LangTrack.Api.Data.SeedData.SeedAsync(context);
}

app.Run();
