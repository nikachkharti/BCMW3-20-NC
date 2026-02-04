using Forum.API.Data;
using Forum.API.Entities;
using Forum.API.Middleware;
using Forum.API.Repository;
using Forum.API.Services;
using Forum.API.Services.Mapping;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;

namespace Forum.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Information("Staring Forum.API");

            var builder = WebApplication.CreateBuilder(args);

            //Controllers
            builder.Services.AddControllers();

            //Swagger


            //builder.Services.AddSwaggerGen(options =>
            //{
            //    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
            //    {
            //        Name = "Authorization",
            //        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
            //        In = ParameterLocation.Header,
            //        Type = SecuritySchemeType.ApiKey,
            //        Scheme = JwtBearerDefaults.AuthenticationScheme
            //    });

            //    options.AddSecurityRequirement(
            //            new OpenApiSecurityRequirement()
            //            {
            //                {
            //                    new OpenApiSecurityScheme
            //                    {
            //                        Reference = new OpenApiReference
            //                        {
            //                            Type = ReferenceType.SecurityScheme,
            //                            Id = JwtBearerDefaults.AuthenticationScheme
            //                        }
            //                    },
            //                    new string[]{}
            //                }
            //            }
            //        );

            //});


            //DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerLocalConnection")));

            //Repository
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();

            //Service
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IAuthService, AuthService>();


            //Serilog
            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            //Mapster
            var config = new TypeAdapterConfig();
            MappingConfig.RegisterMappings(config);
            builder.Services.AddSingleton(config);
            builder.Services.AddScoped<IMapper, ServiceMapper>();

            //Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;

                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.UseDbAutoUpdate();
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
