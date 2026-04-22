using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Terminals.WebApi.Application.Interfaces;

namespace Terminals.WebApi.Endpoints;

public static class TerminalsEndpoint
{
    public static void MapTerminalsEndpoints(this IEndpointRouteBuilder routes)
    {
        var versionSet = routes.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1, 0))
            .ReportApiVersions()
            .Build();

        var group = routes.MapGroup("/api/")
            .WithApiVersionSet(versionSet)
            .HasApiVersion(new ApiVersion(1, 0))
            .WithTags("Terminals queries"); ;

        group.MapGet("terminals", async(
                [FromQuery] string city,
                [FromQuery] string region,
                ITerminalService terminalService,
                HttpContext context)
            =>
        {
            var result = await terminalService.GetOfficesByCityAndRegionAsync(
                city: city,
                region: region,
                context.RequestAborted);

            if (!result.Ok)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Result);
        })
        .Produces(200)
        .ProducesProblem(401)
        .WithName("GetOfficesByCityAndRegionEndpoint")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Search for terminals by city and region name",
            Description = "Returns a list of offices/terminals found in the specified city"
        });

        group.MapGet("city-id", async (
        [FromQuery] string city,
        [FromQuery] string region,
        ITerminalService terminalService,
        HttpContext context)
            =>
                {
                    var result = await terminalService.GetCityIdByCityAndRegionAsync(
                        city: city,
                        region: region,
                        context.RequestAborted);

                    if (!result.Ok)
                        return Results.BadRequest(result.Error);

                    return Results.Ok(result.Result);
                })
        .Produces(200)
        .ProducesProblem(401)
        .WithName("GetCityIdByCityAndRegionEndpoint")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Search for city ID by city and region name",
            Description = "Returns the city ID from the found Office entity"
        });
    }
}