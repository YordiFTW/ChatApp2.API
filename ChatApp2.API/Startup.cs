using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Bussiness.Services;
using ChatApp2.Domain.DbContexts;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
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

            

            services.AddDbContext<ChatAppDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ChatAppDbContext")));

            services.AddIdentity<User, IdentityRole>(options => { })
                .AddEntityFrameworkStores<ChatAppDbContext>()
                .AddDefaultTokenProviders();

            services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

            services.AddScoped<IUserStore<User>, 
                UserOnlyStore<User, ChatAppDbContext>>();
            services.AddScoped<IUserClaimsPrincipalFactory<User>,
                CustomUserClaimsPrincipalFactory>();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
                    options.TokenLifespan = TimeSpan.FromHours(3));

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(o =>
            //{
            //    o.Authority = Configuration["Jwt:Authority"];
            //    o.Audience = Configuration["Jwt:Audience"];
            //});



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                //.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options))
                //.AddJwtBearer(options =>
                //{
                //    options.Audience = "http://localhost:5001/";
                //    options.Authority = "http://localhost:5000/";
                //})
                .AddCookie("cookies", options => options.LoginPath = "/User/Login")
                .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAd"));

            //services.ConfigureApplicationCookie(options => options.LoginPath = "/User/Login");

            services.AddHttpContextAccessor();

            services.AddScoped<IChatDataService, ChatDataService>();
            services.AddScoped<IGroupDataService, GroupDataService>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<HttpContextAccessor>();
            services.AddScoped<IGenericRepository<Chat>, GenericRepository<Chat>>();
            services.AddScoped<IGenericRepository<Group>, GenericRepository<Group>>();
            

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                        
                });
            });

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
            app.UseCors("default");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
