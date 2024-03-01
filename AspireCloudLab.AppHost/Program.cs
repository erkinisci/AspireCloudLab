var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("redis-aspirational");

var grafana = builder.AddContainer("grafana", "grafana/grafana")
    .WithVolumeMount("../grafana/config", "/etc/grafana")
    .WithVolumeMount("../grafana/dashboards", "/var/lib/grafana/dashboards")
    .WithEndpoint(containerPort: 3000, hostPort: 3000, name: "grafana-http", scheme: "http");

builder.AddContainer("prometheus", "prom/prometheus")
    .WithVolumeMount("../prometheus", "/etc/prometheus")
    .WithEndpoint(9090, hostPort: 9090);

var endpointReference = grafana.GetEndpoint("grafana-http");

var apiService = builder
    .AddProject<Projects.AspireCloudLab_ApiService>("apiservice")
    .WithEnvironment("GRAFANA_URL", endpointReference);

builder.AddProject<Projects.AspireCloudLab_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiService)
    .WithEnvironment("GRAFANA_URL", endpointReference);

//builder.Build().Run();
using var app = builder.Build();
await app.RunAsync();
