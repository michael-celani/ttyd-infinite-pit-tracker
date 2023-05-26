using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddRazorPages();
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
// app.UseStaticFiles();

app.UseWebSockets();

app.UseRouting();
app.UseAuthorization();

// app.MapRazorPages();
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();