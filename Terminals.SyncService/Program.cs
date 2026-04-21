using Terminals.AddressParser.Extensions;
using Terminals.SyncService;
using Terminals.SyncService.Definitions.Common;
using Terminals.SyncService.Definitions.DbContext;
using Terminals.SyncService.Definitions.Logger;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddDellinDictionaryDbContext(builder.Configuration);

builder.Logging.AddStructuredLogging();

builder.Services.AddApplicationServices();

builder.Services.AddDefaultAdressParser();

var host = builder.Build();


host.Run();
