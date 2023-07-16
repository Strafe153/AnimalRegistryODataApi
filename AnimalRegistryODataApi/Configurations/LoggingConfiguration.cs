namespace AnimalRegistryODataApi.Configurations;

public static class LoggingConfiguration
{
    public static void ConfigureLogging(this ILoggingBuilder logginBuilder)
    {
        logginBuilder
            .ClearProviders()
            .AddLog4Net("log4net.config");
    }
}
