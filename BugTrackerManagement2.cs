using Microsoft.EntityFrameworkCore;
using BugTrackerManagement.DAL;
using BugTrackerManagement.Services;
using BugTrackerManagement.Exceptions;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text;
namespace BugTrackerManagement
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var allowedOrigins = _configuration.GetSection("AllowedOrigins").Get<string[]>();

            services.AddControllers(options => { options.Filters.Add(new GeneralExceptionHandler()); });

            services.AddDbContext<BugTrackerCatalogContext>(options => { options.UseSqlServer(_configuration.GetConnectionString("Development")); });

            services.AddCors(options => {
                options.AddPolicy("App_Cors_Policy", builder => {
                    builder
                        .WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddScoped<IAdminServices, AdminServices>();
            services.AddScoped<IProjectServices, ProjectServices>();
            services.AddScoped<IBugServices, BugServices>();
            services.AddScoped<IMessageServices, MessageServices>();
            services.AddScoped<IAuthorizationServices, AuthorizationServices>();

            services.AddSwaggerGen(config => { config.SwaggerDoc("v1.0.0", new OpenApiInfo { Title = "Bug Tracker API Show" }); });

            services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:JwtSecret").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddAuthorization(options => {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Roles", "Admin"));
                options.AddPolicy("Developer", policy => policy.RequireClaim("Roles", "Developer", "Admin"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(config => {
                    config.SwaggerEndpoint("/swagger/v1.0.0/swagger.json", "Bug Tracker");
                });
            }

            app.UseCors("App_Cors_Policy");

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}
