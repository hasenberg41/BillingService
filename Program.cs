using GrpcBillingService;
using GrpcBillingService.Interfaces;
using GrpcBillingService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IBillingDataService, DefaultData>();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<BillingService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
