using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Domain.DbContexts;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace ChatApp2.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = @"Data Source=(localdb)\\mssqllocaldb;Database=ChatApp2DB;Trusted_Connection=True;MultipleActiveResultSets=true";

            services.AddIdentityCore<User>(options => { });
            services.AddScoped<IUserStore<User>, UserStore>();

            services.AddAuthentication("cookies")
                .AddCookie("cookies", options => options.LoginPath = "/Home/Login")
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            services.AddDbContext<ChatAppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ChatAppDbContext")));

            services.AddScoped<IChatRepository, ChatRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatApp2.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApp2.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
