using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ProductReport.Components;
using ProductReport.Features.Sales.Services;
using ProductReport.Features.Sales.ViewModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Register our services
builder.Services.AddScoped<ISalesDataService, SalesDataService>();
builder.Services.AddScoped<SalesViewModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
