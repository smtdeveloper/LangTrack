using Microsoft.Extensions.DependencyInjection;
using LangTrack.Application.Services;

namespace LangTrack.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IWordService, WordService>();
        services.AddScoped<IStudyLogService, StudyLogService>();
        services.AddScoped<IStatsService, StatsService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRoleService, RoleService>();

        return services;
    }
}
