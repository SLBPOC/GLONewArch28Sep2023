using Delfi.Glo.Api.Middleware;
using Delfi.Glo.Common.Models;
using Delfi.Glo.Common.Services;
using Delfi.Glo.Common.Services.Interfaces;
using Delfi.Glo.Entities.Dto;
using Delfi.Glo.PostgreSql.Dal;
using Delfi.Glo.PostgreSql.Dal.Services;
using Delfi.Glo.DataAccess.Services;
using Delfi.Glo.Repository;

namespace Delfi.Glo.Api.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IWellService<WellDetailsDto>, WellService>();
            services.AddScoped<IWellInfoService<WellInfoDto>, WellInfoService>();
            services.AddScoped<IAlertService<AlertsDetailsDto>, AlertsService>();
            services.AddScoped<IEventService<EventDetailsDto>, EventService>();
            services.AddScoped<IUniversityService<UniversitiesDto>, UniversityService>();
            services.AddScoped<ICustomAlertService<CustomAlertDetailsDto>, CustomAlertService>();
            services.AddScoped<IWellHierarchyService, WellHierarchyService>();
            services.AddScoped(typeof(IHttpService<>), typeof(HttpService<>));
            services.AddTransient<ExceptionMiddleware>();
            services.AddScoped<HttpClient>(s => new HttpClient());
            var baseUrls = configuration.GetSection("BaseUrls").Get<BaseUrls>();
            if(baseUrls != null)
                services.AddSingleton(baseUrls);
            return services;
        }
    }
}
