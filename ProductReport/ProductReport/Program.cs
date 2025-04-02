using ProductReport.Components;
using ProductReport.Configurations;
using ProductReport.Features.Sales.Models;
using ProductReport.Features.Sales.Services;
using ProductReport.Features.Sales.ViewModels;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.Configure<UploadSettings>(
    builder.Configuration.GetSection("UploadSettings"));

builder.Services.AddScoped<ISalesDataService, SalesDataService>();
builder.Services.AddScoped<SalesViewModel>();

builder.Services.AddScoped<ICsvParser<SalesRecord>, SalesCsvParser>();
builder.Services.AddScoped<IFileReaderService, FileReaderService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
