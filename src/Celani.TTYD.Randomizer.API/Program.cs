using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
    builder.Services.AddRazorPages();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
    app.UseStaticFiles();

var options = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(0)
};

app.UseWebSockets(options);

app.UseRouting();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
    app.MapRazorPages();

app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();