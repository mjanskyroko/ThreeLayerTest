using TestWebApp;
using Microsoft.AspNetCore.Hosting;

public static partial class Program
{
    public static async Task Main(string[] args) => await ApplicationLauncher.RunAsync<Startup>(args);
}

/*
var builder = WebApplication.CreateBuilder(args);

MssqlAdapterSettings MssqlSettings 

builder.Services.AddApplicationLayer();
builder.Services.add
builder.Services.AddServices();

// Add services to the container.
builder.Services.AddRazorPages();

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

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
*/