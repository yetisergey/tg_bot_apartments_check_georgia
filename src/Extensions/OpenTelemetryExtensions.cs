using System.Reflection;
using Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Extensions;

public static class OpenTelemetryExtensions
{
    public static void AddOpenTelemetry(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment,
        ConfigureHostBuilder host)
    {
        var assemblyFullName = Assembly.GetCallingAssembly().GetName().Name;
        var otelConfiguration = configuration.GetRequiredSection(nameof(OtelConfiguration))
            .Get<OtelConfiguration>();

        services.AddOpenTelemetryTracing(builder =>
        {
            builder
                .AddConsoleExporter()
                .AddOtlpExporter(opt =>
                {
                    opt.Protocol = OtlpExportProtocol.Grpc;
                    opt.Endpoint = new Uri($"http://{otelConfiguration.HostName}:4317");
                })
                .AddSource(assemblyFullName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(assemblyFullName).AddTelemetrySdk())
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddSqlClientInstrumentation(options =>
                {
                    options.SetDbStatementForText = true;
                    options.RecordException = true;
                });
        });

        services.AddOpenTelemetryMetrics(builder =>
        {
            builder
            .AddConsoleExporter()
            .AddMeter(assemblyFullName)
            .AddOtlpExporter(opt =>
            {
                opt.Protocol = OtlpExportProtocol.Grpc;
                opt.Endpoint = new Uri($"http://{otelConfiguration.HostName}:4317");
            })
            .SetResourceBuilder(
                ResourceBuilder.CreateDefault()
                    .AddService(assemblyFullName).AddTelemetrySdk())
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation();
        });
    }
}
