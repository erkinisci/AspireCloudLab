using System.Diagnostics.Metrics;

namespace AspireCloudLab.WeatherApp.Api.CustomMetrics;

public class WeatherMetrics
{
    public const string MeterName = "WeatherApp.Api";
    
    private readonly Counter<long> _weatherRequestCounter;
    private readonly Histogram<double> _weatherRequestDuration;

    public WeatherMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _weatherRequestCounter = meter.CreateCounter<long>($"{MeterName.ToLower()}.weather_requests.count");

        _weatherRequestDuration = meter.CreateHistogram<double>($"{MeterName.ToLower()}.weather_requests.duration", "ms");
    }
    
    public void IncreaseWeatherRequestCount() => _weatherRequestCounter.Add(1);
    
    public TrackedRequestDuration MeasureRequestDuration() => new(_weatherRequestDuration);
}