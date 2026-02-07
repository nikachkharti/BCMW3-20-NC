using Forum.API.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Forum.API
{
    public static class Extensions
    {
        public static void UseDbAutoUpdate(this WebApplication app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();

                Log.Information("Database updated.");
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while updating the database: {Message}", ex.Message, ex);
            }
        }
    }
}
