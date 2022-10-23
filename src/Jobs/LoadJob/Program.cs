using Extensions;
using Hangfire;
using LoadJob.Extensions;
using LoadJob.Filters;
using LoadJob.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;
var host = builder.Host;

services.AddOpenTelemetry(config, env, host);
services.AddServices(config);

var app = builder.Build();

//var scope = app.Services.CreateScope();
//var syncService = scope.ServiceProvider.GetRequiredService<ISyncService>();
//var cityService = scope.ServiceProvider.GetRequiredService<ICityService>();
//var cities = await cityService.Get();
//foreach (var cityDTO in cities)
//{
//    await syncService.SyncFlatsAsync(cityDTO);
//}

app.UseHangfireDashboard(options: new DashboardOptions
{
    Authorization = new[] { new DashboardAuthorizationFilter() },
});
app.UseHttpsRedirection();

RecurringJob.AddOrUpdate<IJobService>(nameof(IJobService), (job) => job.ExecuteAsync(), "*/10 * * * *");

app.Run();
