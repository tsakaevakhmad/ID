﻿using Microsoft.Extensions.FileProviders;

namespace ID.Extensions
{
    public static class ClientExtension
    {
        public static void AddFrontEnd(this WebApplication app)
        {
            var staticFilesPath = app.Configuration["FrontRootFolder"];
            if (!string.IsNullOrEmpty(staticFilesPath))
                ClientByPass(app, staticFilesPath);
            else
                ClientByDefaultPass(app);
        }

        private static void ClientByPass(WebApplication app, string root)
        {
            
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                FileProvider = new PhysicalFileProvider(root),
                RequestPath = ""
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(root),
                RequestPath = ""
            });

            app.MapFallback(() => Results.File(
                Path.Combine(root, "index.html"),
                "text/html"
            ));
        }

        private static void ClientByDefaultPass(WebApplication app)
        {

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.MapFallbackToFile("index.html");
        }
    }
}
