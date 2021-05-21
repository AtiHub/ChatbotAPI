using ChatbotAPI.Data;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatbotAPI.Helpers;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ChatbotAPI.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace ChatbotAPI {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddCors(options => {
                options.AddPolicy("AllowAll", p => {
                    p.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            string mySqlConnectionStr = Configuration.GetConnectionString("MySQLConnectionString");
            services.AddDbContextPool<ChatbotAPIContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<JwtMiddleware>();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
