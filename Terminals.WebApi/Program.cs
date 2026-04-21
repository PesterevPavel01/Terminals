using Terminals.WebApi.Endpoints;
using Terminals.WebApi.Extension.DbContext;
using Terminals.WebApi.Extensions.Services;
using Terminals.WebApi.Extensions.Swagger;
using Terminals.WebApi.Logger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwagger();

builder.Services.AddDellinDictionaryDbContext(builder.Configuration);

builder.Logging.AddStructuredLogging();

builder.Services.AddApplicationServices();

var app = builder.Build();

app.MapTerminalsEndpoints();

app.UseSwaggerWithUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
