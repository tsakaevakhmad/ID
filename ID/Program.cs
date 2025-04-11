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

app.UseCors(options =>
{
    options.AllowAnyHeader();
    options.AllowAnyMethod();
    options.AllowCredentials();
    options.WithOrigins(app.Configuration["FrontendRoute"]);
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
