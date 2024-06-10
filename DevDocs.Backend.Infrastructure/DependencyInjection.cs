using DevDocs.Backend.Application.Interfaces;
using DevDocs.Backend.Domain.Abstractions;
using DevDocs.Backend.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using System.Security.Claims;
using DevDocs.Backend.Application;
using DevDocs.Backend.Application.Abstractions;
using DevDocs.Backend.Infrastructure.Authentication;
using DevDocs.Backend.Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using DevDocs.Backend.Application.Abstractions.Clock;
using DevDocs.Backend.Infrastructure.Clock;

namespace DevDocs.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        AddSwagger(services);
        AddPersistence(services, configuration);
        AddAuthentication(services, configuration);
        AddAuthorization(services);
        AddRepositories(services);
        AddMediatR(services);

        services.AddApplication();
        services.AddHttpClient();
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        return services;
    }

    private static void AddPersistence(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Database") ??
                                  throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(connectionString));
        services.AddScoped<IUnitOfWork, ApplicationDbContext>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
    }

    private static void AddAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = (context) =>
                    {
                        if (context.Principal?.HasClaim(claim => claim.Type == "realm_access") == true)
                        {
                            var realmAccessClaimValue = context.Principal.Claims
                                .FirstOrDefault(claim => claim.Type == "realm_access")?.Value;

                            if (!string.IsNullOrEmpty(realmAccessClaimValue))
                            {
                                var values =
                                    JsonConvert.DeserializeObject<Dictionary<string, object>>(realmAccessClaimValue);
                                if (values != null && values.TryGetValue("roles", out var roles))
                                {
                                    var rolesArray = JArray.FromObject(roles);
                                    var result =
                                        rolesArray?.FirstOrDefault(val => val.Value<string>() == "dev-docs-admin");
                                    if (result != null)
                                    {
                                        var claims = new List<Claim>
                                        {
                                            new Claim(ClaimTypes.Role, "admin")
                                        };
                                        var appIdentity = new ClaimsIdentity(claims);
                                        context.Principal?.AddIdentity(appIdentity);
                                    }
                                }
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));


        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

        services.AddTransient<AdminAuthorizationDelegatingHandler>();

        services.AddHttpClient<IAuthenticationService, AuthenticationService>((serviceProvider, httpClient) =>
        {
            var keycloakOptions = serviceProvider.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            httpClient.BaseAddress = new Uri(keycloakOptions.AdminUrl);
        }).AddHttpMessageHandler<AdminAuthorizationDelegatingHandler>();



        services.AddHttpContextAccessor();
    }

    private static void AddAuthorization(IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("admin", policy =>
                policy
                    .RequireRole("admin"));

        services.AddAuthorization();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field. Example: Bearer {your token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddTransient<IUserRepository, UserRepository>();
    }
    private static void AddMediatR(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
    }
}