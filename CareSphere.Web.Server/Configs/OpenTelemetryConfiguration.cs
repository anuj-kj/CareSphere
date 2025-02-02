using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Azure.Monitor.OpenTelemetry.Exporter;
using System.Diagnostics;
using OpenTelemetry;

namespace CareSphere.Web.Server.Configs
{
    public static class OpenTelemetryConfiguration
    {
        public static void ConfigureOpenTelemetry(WebApplicationBuilder builder)
        {
            var isTracingEnabled = builder.Configuration.GetValue<bool>("AzureMonitor:Enabled", false);
            var connectionString = builder.Configuration["AzureMonitor:ConnectionString"];

            if (isTracingEnabled && !string.IsNullOrEmpty(connectionString))
            {
                var activitySource = new ActivitySource("CareSphereAPI");

                builder.Services.AddOpenTelemetry()
                    .ConfigureResource(resource => resource.AddService("CareSphereAPI"))
                    .WithTracing(tracerProviderBuilder =>
                    {
                        tracerProviderBuilder
                            .AddSource("CareSphereAPI") // ✅ Add custom activity source
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddEntityFrameworkCoreInstrumentation(options =>
                            {
                                options.SetDbStatementForText = true;  // ✅ Capture SQL queries
                            })
                            .AddProcessor(new CustomActivityProcessor()) // ✅ Custom Processor to Add Duration
                            .SetSampler(new TraceIdRatioBasedSampler(0.1)) // ✅ Sample 10% of traces
                            .AddAzureMonitorTraceExporter(options =>
                            {
                                options.ConnectionString = connectionString;
                            });
                    })
                    .WithMetrics(meterProviderBuilder =>
                    {
                        meterProviderBuilder
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddAzureMonitorMetricExporter(options =>
                            {
                                options.ConnectionString = connectionString;
                            });
                    });
            }
        }
    }

    // ✅ Custom Processor to Add Slow Request Tag
    public class CustomActivityProcessor : BaseProcessor<Activity>
    {
        public override void OnEnd(Activity activity)
        {
            if (activity.Duration.TotalMilliseconds > 2000) // ✅ Requests >2s
            {
                activity.SetTag("http.slow_request", true);
                activity.AddEvent(new ActivityEvent("Slow API Request Detected"));
            }
        }
    }
}
