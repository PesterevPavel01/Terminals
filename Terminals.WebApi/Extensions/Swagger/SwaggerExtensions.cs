using Asp.Versioning;
using Microsoft.OpenApi.Models;

namespace Terminals.WebApi.Extensions.Swagger;

public static class SwaggerExtensions
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services
            .AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-api-version"),
                    new UrlSegmentApiVersionReader());
            })
            .AddApiExplorer(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            })
            .AddApiExplorer(options =>
             {
                 options.GroupNameFormat = "'v'VVV";
                 options.SubstituteApiVersionInUrl = true;
             });

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "Сервис Terminal.WebApi",
                Description = "Версия 1.0",
            });
        });
    }

    public static void UseSwaggerWithUI(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1.0");
            });
        }
    }
}