using Anirut.Evacuation.Api.Data;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<DataContext>("exampleDB");

builder.Services.AddCors(options =>
{
    const string DefaultCorsPolicy = "DefaultCorsPolicy";
    options.AddPolicy(DefaultCorsPolicy, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();
builder.Services.AddFastEndpoints();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", () => Results.Redirect("scalar")).ExcludeFromDescription();

app.UseHttpsRedirection();

app.UseCors("DefaultCorsPolicy");

app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = "api";
    config.Versioning.Prefix = "v";
    config.Versioning.PrependToRoute = true;
});


app.Run();
