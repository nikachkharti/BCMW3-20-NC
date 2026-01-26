using Forum.API.Data;
using Forum.API.Repository;
using Forum.API.Services;
using Forum.API.Services.Mapping;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Forum.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerLocalConnection")));
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ITopicService, TopicService>();

            //Mapster
            var config = new TypeAdapterConfig();
            MappingConfig.RegisterMappings(config);
            builder.Services.AddSingleton(config);
            builder.Services.AddScoped<IMapper, ServiceMapper>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
