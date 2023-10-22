using Microsoft.EntityFrameworkCore;

namespace Api;

public class DataContextAutomaticMigrationStartupFilter<T> : IStartupFilter where T : DbContext
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<T>().Database.Migrate();
            }
            next(app);
        };
    }
}
