using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Delfi.Glo.Common.Constants;

namespace Delfi.Glo.Api.Middleware
{
    public static class AuthenticationMiddleware
    {
        public static void AddSAuthAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                string openIdConfigUrl = $"{Environment.GetEnvironmentVariable("IdentityProviderUrl")!}{UrlConstants.SAuthOpenIdConfigUrl}";
                List<string> audience = new List<string>();
                audience.Add(Environment.GetEnvironmentVariable("IdentityProviderClientKey")!);

                options.MetadataAddress = openIdConfigUrl;
                options.TokenValidationParameters.ValidAudiences = audience;
                options.AutomaticRefreshInterval = TimeSpan.FromHours(1);
            });
        }

        public static void AddSwaggerSAuthentication(this IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                string securityRequirementId = "Authentication Token";

                swagger.UseOneOfForPolymorphism();
                swagger.UseAllOfForInheritance();

                // Add security definition to swagger.
                swagger.AddSecurityDefinition(securityRequirementId, new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Description = "SAuth Authentication Token",
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = securityRequirementId
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            Description = "Please pass the access token here",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                        },
                        new List<string>()
                    }
                });
            });
        }
    }
}
