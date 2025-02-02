using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Azure.Monitor.OpenTelemetry.Exporter;
using System.Diagnostics;

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
                // ✅ Register Activity Listener for Custom Tags
                RegisterActivityListener();

                builder.Services.AddOpenTelemetry()
                    .ConfigureResource(resource => resource.AddService("CareSphereAPI"))
                    .WithTracing(tracerProviderBuilder =>
                    {
                        tracerProviderBuilder
                            .AddSource("CareSphereAPI") // ✅ Custom activity source
                            .AddAspNetCoreInstrumentation()
                            .AddHttpClientInstrumentation()
                            .AddEntityFrameworkCoreInstrumentation(options =>
                            {
                                options.SetDbStatementForText = true;  // ✅ Capture SQL queries
                                options.EnrichWithIDbCommand = (activity, command) =>
                                {
                                    var durationMs = activity.Duration.TotalMilliseconds;
                                    if (durationMs > 1000) // ✅ Log only if SQL query takes longer than 1s
                                    {
                                        activity.SetTag("sql.slow_query", true);
                                        activity.SetTag("sql.query_text", command.CommandText);
                                        activity.SetTag("sql.duration_ms", durationMs);
                                    }
                                };
                            })
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

        // ✅ Custom Activity Listener to Add http.slow_request Tag
        private static void RegisterActivityListener()
        {
            var listener = new ActivityListener
            {
                ShouldListenTo = source => true,
                Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllData,
                ActivityStopped = activity =>
                {
                    if (activity.Duration.TotalMilliseconds > 2000) // ✅ Requests >2s
                    {
                        activity.SetTag("http.slow_request", true);
                        activity.AddEvent(new ActivityEvent("Slow API Request Detected"));
                    }
                }
            };

            ActivitySource.AddActivityListener(listener);
        }
    }
}
