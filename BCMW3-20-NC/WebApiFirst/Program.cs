using WebApiFirst.Services;

namespace WebApiFirst
{
    public class Program
    {
        public static void Main(string[] args)
        {
            #region Container

            var builder = WebApplication.CreateBuilder(args); // ქმნის ვებისთვის საჭირო ინფრასტრუქტურას


            builder.Services.AddControllers(); // ქმნის კონტროლერების მენეჯმენტ სერვისს
            builder.Services.AddOpenApi(); // ქმნის OpenAPI სერვისს 
            builder.Services.AddScoped<INotificationService, EmailService>();
            builder.Services.AddTransient<UserService>();


            var provider = builder.Services.BuildServiceProvider();


            using (var scope1 = provider.CreateScope())
            {
                var service1 = scope1.ServiceProvider.GetRequiredService<UserService>();
                var service2 = scope1.ServiceProvider.GetRequiredService<UserService>();

                service1.Register();
                service2.Register();
            }


            using (var scope2 = provider.CreateScope())
            {
                var service3 = scope2.ServiceProvider.GetRequiredService<UserService>();

                service3.Register();
            }



            #endregion

            #region Middleware

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();

            #endregion
        }
    }
}
