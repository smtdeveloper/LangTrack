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
    
    // JWT Authentication için Swagger yapılandırması
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add application services
builder.Services.AddApplication();

// Add infrastructure services
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=langtrack.db";
builder.Services.AddInfrastructure(connectionString);

// Add JWT Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "LangTrack",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "LangTrack.Users",
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "LangTrack_Super_Secret_Key_2024_Minimum_32_Characters"))
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Add permission-based policies
    options.AddPolicy("words:create", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("words", "create")));
    options.AddPolicy("words:read", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("words", "read")));
    options.AddPolicy("words:update", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("words", "update")));
    options.AddPolicy("words:delete", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("words", "delete")));
    
    options.AddPolicy("study:create", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("study", "create")));
    options.AddPolicy("study:read", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("study", "read")));
    
    options.AddPolicy("stats:read", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("stats", "read")));
    
    options.AddPolicy("users:read", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("users", "read")));
    options.AddPolicy("users:update", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("users", "update")));
    options.AddPolicy("users:delete", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("users", "delete")));
    
    options.AddPolicy("roles:create", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("roles", "create")));
    options.AddPolicy("roles:read", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("roles", "read")));
    options.AddPolicy("roles:update", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("roles", "update")));
    options.AddPolicy("roles:delete", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("roles", "delete")));
    
    options.AddPolicy("permissions:read", policy => policy.Requirements.Add(new LangTrack.Api.Authorization.PermissionRequirement("permissions", "read")));
});

// Register permission handler
builder.Services.AddScoped<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, LangTrack.Api.Authorization.PermissionHandler>();

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

// Authentication & Authorization
app.UseAuthentication();
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
