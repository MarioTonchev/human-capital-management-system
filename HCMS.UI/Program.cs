using HCMS.UI.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Adding services to the container.

builder.Services.AddControllersWithViews();
builder.Services.ConfigureHttpClient();
builder.Services.ConfigureSession();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureUIServices();

// Specifying the URLs for the application to listen on.

builder.WebHost.UseUrls("https://localhost:7039", "http://localhost:5139");

// Building the application.

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Configuring middleware pipeline.

app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();