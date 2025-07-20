using HCMS.BackendAPI.Extensions;
using HCMS.Infrastructure.Data.SeedDb;

var builder = WebApplication.CreateBuilder(args);

// Adding services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJwtAuth(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureDependencies();

// Specifying the URLs for the application to listen on.

builder.WebHost.UseUrls("https://localhost:7248", "http://localhost:5041");

// Building the application.

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("https://localhost:7248/swagger/v1/swagger.json", "HCM API v1");
        options.RoutePrefix = string.Empty;
    });
}

// Seeding initial data.

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        await DbSeeder.SeedAsync(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during seeding the identity database.");

        throw;
    }
}

// Configuring middleware pipeline.

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();