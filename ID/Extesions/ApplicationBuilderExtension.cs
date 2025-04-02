using ID.Data;
using Microsoft.EntityFrameworkCore;

namespace ID.Extesions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder Migrate(this IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PgDbContext>();
                dbContext.Database.Migrate();
            }

            return applicationBuilder;
        }
    }
}
