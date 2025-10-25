using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LangTrack.Infrastructure.Data;
using LangTrack.Infrastructure.Repositories;
using LangTrack.Application.Interfaces;

namespace LangTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<LangTrackDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IWordRepository, WordRepository>();
        services.AddScoped<IStudyLogRepository, StudyLogRepository>();

        return services;
    }
}
