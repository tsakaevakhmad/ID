using ID.Extensions;
using ID.Extesions;
using Microsoft.EntityFrameworkCore;

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
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); 
});
app.AddFrontEnd();
app.Run();
