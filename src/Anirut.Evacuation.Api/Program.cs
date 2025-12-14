using Anirut.Evacuation.Api.Configurations;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConfigurations(builder.Configuration);


builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();

var app = builder.Build();
app.Services.CreateDatabase();
app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", () => Results.Redirect("scalar")).ExcludeFromDescription();

app.UseCors("DefaultCorsPolicy");

app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
    config.Versioning.Prefix = "v";
    config.Versioning.PrependToRoute = true;
});

app.Run();
