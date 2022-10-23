using Extensions;
using NotifyJob.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;
var host = builder.Host;

services.AddOpenTelemetry(config, env, host);
services.AddServices(config);

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
