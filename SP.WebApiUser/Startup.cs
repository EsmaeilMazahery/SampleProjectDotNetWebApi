using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SP.DataLayer.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FirebaseAdmin.Messaging;
using System.Text;
using SP.DomainLayer.Models;
using SP.ServiceLayer.Services;
using SP.WebApiMember.Extension;
using SP.WebApiMember.Extension.ActionFilters;
using SP.WebApiMember.Extension.Middlewares;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StructureMap;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;
using Newtonsoft.Json;

namespace SP.WebApiMember
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
            services.AddDbContext<DatabaseContext>(options =>
                               options.UseSqlServer(this.Configuration.GetConnectionString("ServerDb"),
                               b => b.MigrationsAssembly("SP.DataLayer")));

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                              .AllowAnyMethod()
                                              .AllowAnyHeader().Build();
                                  }));

            services.AddDistributedMemoryCache();

            services.AddControllers();

            services = AddOAuthProviders(services);

            ConfigureIoC(services);
        }


        public IServiceCollection AddOAuthProviders(IServiceCollection services)
        {
            services.AddAuthentication(x =>
                       {
                           x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                           x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                       }).AddJwtBearer(options =>
                           {
                               options.Events = new JwtBearerEvents
                               {
                                   OnTokenValidated = context =>
                                   {
                                       var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                                       var userId = int.Parse(context.Principal.Identity.Name);
                                       var user = userService.Read(userId);
                                       if (user == null)
                                       {
                                           // return unauthorized if user no longer exists
                                           context.Fail("Unauthorized");
                                       }
                                       else
                                       {

                                       }

                                       return Task.CompletedTask;
                                   },
                               };
                               options.RequireHttpsMetadata = false;
                               options.SaveToken = true;
                               options.TokenValidationParameters = new TokenValidationParameters
                               {
                                   ValidateIssuerSigningKey = true,
                                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Security.secretKey)),
                                   ValidateIssuer = false,
                                   ValidateAudience = false
                               };

                           });

            return services;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            var googleCredential = env.ContentRootPath;
            var filePath = Configuration.GetSection("GoogleFirebase")["filename"];
            googleCredential = Path.Combine(googleCredential, filePath);
            var credential = GoogleCredential.FromFile(googleCredential);
            FirebaseApp.Create(new AppOptions()
            {
                Credential = credential
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
            }
        }

        internal class Lazier<T> : Lazy<T> where T : class
        {
            public Lazier(IServiceProvider provider)
                : base(() => provider.GetRequiredService<T>())
            {
            }
        }

        public void ConfigureIoC(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, DatabaseContext>(provider => new DatabaseContext(provider.GetService<DbContextOptions<DatabaseContext>>()));
            services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));
            services.AddTransient<ICallRepository, CallRepository>();
            services.AddTransient<IMediaRepository, MediaRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IServiceRepository, ServiceRepository>();
            services.AddTransient<ISettingRepository, SettingRepository>();
            services.AddTransient<ISmsRepository, SmsRepository>();
            services.AddTransient<IMemberRepository, MemberRepository>();
            services.AddTransient<IUserVisitRepository, UserVisitRepository>();
            services.AddTransient<IMemberVisitRepository, MemberVisitRepository>();

            services.AddTransient<ILogUserRepository, LogUserRepository>();
        }
    }
}
