using ID.Extensions;
using ID.Extesions;
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

var staticFilesPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../IDClient/idclient/build"));

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(staticFilesPath),
    RequestPath = ""
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticFilesPath),
    RequestPath = ""
});

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
app.MapFallback(() => Results.File(
    Path.Combine(staticFilesPath, "index.html"),
    "text/html"
));


app.Run();
