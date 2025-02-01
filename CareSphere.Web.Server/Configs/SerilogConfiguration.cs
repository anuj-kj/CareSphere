using Serilog;
using Microsoft.ApplicationInsights.Extensibility;

namespace CareSphere.Web.Server.Configs
{
    public static class SerilogConfiguration
    {
        public static void ConfigureSerilog(WebApplicationBuilder builder)
        {
            var isLoggingEnabled = builder.Configuration.GetValue<bool>("ApplicationInsights:Enabled", false);
            var instrumentationKey = builder.Configuration["ApplicationInsights:InstrumentationKey"];

            if (isLoggingEnabled && !string.IsNullOrEmpty(instrumentationKey))
            {
                builder.Host.UseSerilog((context, services, config) =>
                {
                    config
                        .MinimumLevel.Information()  // Log only INFO and above
                        .WriteTo.Console()
                        .WriteTo.ApplicationInsights(
                            new TelemetryConfiguration(instrumentationKey),
                            TelemetryConverter.Traces);
                });
            }
            else
            {
                // Local development: log only to console
                builder.Host.UseSerilog((context, services, config) =>
                {
                    config.MinimumLevel.Debug().WriteTo.Console();
                });
            }
        }
    }
}
