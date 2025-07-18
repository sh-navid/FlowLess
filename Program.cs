using Microsoft.OpenApi.Models;
using NoFlowEngine.Middlewares;
using NoFlowEngine.Extensions;
using NoFlowEngine.Exceptions;
using NoFlowEngine.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Configuration/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"Configuration/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetDefaultConnectionString();
builder.Services.ConfigureDatabase(connectionString);

builder.Services.ConfigureServices();

builder.Services.AddHttpContextAccessor();

// Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NoFlowEngine API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NoFlowEngine API V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

DatabaseInitializer.InitializeDatabase(app);

var logger = app.Services.GetRequiredService<ILogger<Program>>();
app.ConfigureExceptionHandler(logger);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestResponseLogging();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "api",
    pattern: "api/{controller}/{action}/{id?}");

app.Run();