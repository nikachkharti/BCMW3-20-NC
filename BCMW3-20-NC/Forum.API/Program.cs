using CloudinaryDotNet;
using Forum.API.Middleware;
using Forum.Application.Contracts.Repository;
using Forum.Application.Contracts.Service;
using Forum.Application.Jobs;
using Forum.Application.Mapping;
using Forum.Application.Models.Cloudinary;
using Forum.Application.Models.Notification;
using Forum.Application.Services;
using Forum.Domain.Entities;
using Forum.Infrastructure.Data;
using Forum.Infrastructure.Repository;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Quartz;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;

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
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http, // Changed from ApiKey
                    Scheme = "bearer",             // Must be lowercase "bearer"
                    BearerFormat = "JWT",
                    Description = "Just paste your token below (without the 'Bearer ' prefix)"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });



                #region XML documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                #endregion

                #region Examples
                options.ExampleFilters();
                #endregion
            });
            builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

            //DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(
                options => options
                    .UseSqlServer(
                        builder.Configuration.GetConnectionString("SqlServerLocalConnection"),
                        sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(5),
                                errorNumbersToAdd: null);
                        }
                    )
            );

            //Repository
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            //Service
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddSingleton<ISmtpClientWrapper, SmtpClientWrapper>();


            //HttpContextAccessor
            builder.Services.AddHttpContextAccessor();

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

            //Identity Options
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 3;     // lock after 3 fails
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.AllowedForNewUsers = true;
            });



            //Token Authentication logic
            var issuer = builder.Configuration["JwtOptions:Issuer"];
            var secret = builder.Configuration["JwtOptions:Secret"];
            var audience = builder.Configuration["JwtOptions:Audience"];
            var key = Encoding.ASCII.GetBytes(secret);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, // Reject expired tokens
                    //By default, .NET allows a 5-minute grace period after expiration Timespan.Zero means strict expiration
                    //ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = issuer,
                    ValidAudience = audience
                };
            });

            //Account Activation Token Logic
            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(3);
            });

            //Cloudinary
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));

            var cloudinarySettings = builder.Configuration.GetSection("Cloudinary").Get<CloudinarySettings>();
            Console.WriteLine($"Cloudinary Name: {cloudinarySettings.CloudName}");
            Console.WriteLine($"Cloudinary Api Key: {cloudinarySettings.ApiKey}");
            Console.WriteLine($"Cloudinary Api Secret: {cloudinarySettings.ApiSecret}");

            var account = new Account(cloudinarySettings.CloudName, cloudinarySettings.ApiKey, cloudinarySettings.ApiSecret);
            var cloudinary = new Cloudinary(account) { Api = { Secure = true } };
            builder.Services.AddSingleton(cloudinary);
            builder.Services.AddScoped<ICloudinaryImageService, CloudinaryImageService>();

            //Quartz
            builder.Services.AddQuartz(q =>
            {
                //1. UnlockNotificationJob
                var unlockNotificationJobKey = new JobKey("UnlockNotificationJob");
                q.AddJob<UnlockNotificationJob>(options => options.WithIdentity(unlockNotificationJobKey));
                q.AddTrigger(options => options
                    .ForJob(unlockNotificationJobKey)
                    .WithIdentity("UnlockNotificationJob-cron-trigger")
                    .WithCronSchedule("0 */2 * ? * *")
                );
            });
            builder.Services.AddQuartzHostedService(q =>
            {
                q.WaitForJobsToComplete = true;
            });


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
