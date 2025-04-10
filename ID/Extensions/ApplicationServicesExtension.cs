﻿using ID.Data;
using ID.Interfaces;
using ID.MappingProfiles;
using ID.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ID.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection GetMainServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(BaseProfile).Assembly);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.UseInlineDefinitionsForEnums();
            });
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddControllersWithViews()
                            .AddNewtonsoftJson(options =>
                            {
                                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                            });
            return services;
        }

        public static IServiceCollection MockServices(this IServiceCollection services)
        {
            services.AddTransient<IMailService, EmailServiceMock>();
            return services;
        }

        public static WebApplicationBuilder GetServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<PgDbContext>(option =>
            {
                option.UseNpgsql(builder.Configuration.GetSection("Pg").Value);
                option.UseOpenIddict();
            }
);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            return builder;
        }
    }
}
