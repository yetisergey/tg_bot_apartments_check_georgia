using BotApp.Extensions;
using Extensions;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var services = builder.Services;
var env = builder.Environment;
var host = builder.Host;

services.AddOpenTelemetry(config, env, host);
services.AddServices(config);

var app = builder.Build();

//using var scope = app.Services.CreateScope();
//var sp = scope.ServiceProvider;
//var context = sp.GetRequiredService<BotContext>();
//context.Database.EnsureCreated();
//context.Database.Migrate();
//var flatContexts = sp.GetRequiredService<IEnumerable<FlatContext>>();
//foreach (var flatContext in flatContexts)
//{
//    flatContext.Database.EnsureCreated();
//}

app.Run();
