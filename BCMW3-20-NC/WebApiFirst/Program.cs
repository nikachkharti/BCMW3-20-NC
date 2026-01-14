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
