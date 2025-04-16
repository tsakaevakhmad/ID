using ID.Extensions;
using ID.Extesions;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.GetMainServices();

if (builder.Environment.IsDevelopment())
    builder.Services.MockServices();

builder.GetServices();
builder.OpenIddictSettings();
var app = builder.Build();
app.Migrate();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllers();
app.AddFrontEnd();
/*app.UseSpa(spa =>
{
    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
});*/
app.Run();
