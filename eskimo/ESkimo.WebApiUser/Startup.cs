using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiUser.Extension;
using ESkimo.WebApiUser.Extension.ActionFilters;
using ESkimo.WebApiUser.Extension.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StructureMap;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.SignalR;
using ESkimo.WebApiUser.Notifications;
using Microsoft.AspNetCore.Http.Features;
using ESkimo.WebApiUser.Services;

namespace ESkimo.WebApiUser
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options =>
                    options.UseSqlServer(this.Configuration.GetConnectionString("ServerDb"), 
                    b => b.MigrationsAssembly("ESkimo.DataLayer").UseNetTopologySuite()));

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            services = AddOAuthProviders(services);
            
            services.AddSignalR().AddJsonProtocol(options =>
            {
                options.PayloadSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.PayloadSerializerSettings.Converters.Add(new StringEnumConverter());
            });
            services.AddSingleton<IUserIdProvider, UserIdProvider>();

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
                                  {
                                  builder
                                    .AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .AllowCredentials()
                                    .WithOrigins(Configuration["AppSettings:Origin"]);
                                  }));

            services.AddMvc(options =>
            {
                options.Filters.Add(new ModelStateCheckActionFilter());
            }).AddControllersAsServices().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling =
                                           Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddHostedService<KeepAliveHostedService>();
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();


            return ConfigureIoC(services);
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
                                       var user = userService.GetAsync(userId);
                                       if (user.Result == null)
                                       {
                                          // context.Fail($"The claim 'oid' is not present in the token.");
                                           throw new UnauthorizedAccessException();
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


            // services.AddAuthentication()
            //     .AddFacebook(o =>
            //     {
            //         o.AppId = this.Configuration["Authentication:Facebook:AppId"];
            //         o.AppSecret = this.Configuration["Authentication:Facebook:AppSecret"];
            //     });

            // services.AddAuthentication()
            //     .AddGoogle(o =>
            //     {
            //         o.ClientId = this.Configuration["Authentication:Google:ClientId"];
            //         o.ClientSecret = this.Configuration["Authentication:Google:ClientSecret"];
            //     });
            // services.AddAuthentication()
            //     .AddTwitter(o =>
            //     {
            //         o.ConsumerKey = this.Configuration["Authentication:Twitter:ConsumerKey"];
            //         o.ConsumerSecret = this.Configuration["Authentication:Twitter:ConsumerSecret"];
            //     });

            // services.AddAuthentication()
            //     .AddMicrosoftAccount(o =>
            //     {
            //         o.ClientId = this.Configuration["Authentication:Microsoft:ClientId"];
            //         o.ClientSecret = this.Configuration["Authentication:Microsoft:ClientSecret"];
            //     });

            return services;
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IUnitOfWork uow)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseCors("MyPolicy");
            app.UseMvc();
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationsHub>("/notifications");
            });

            app.UseMvcWithDefaultRoute();

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
            }

            uow.initdb();
        }

        public IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            var container = new Container();

            container.Configure(config =>
            {
                config.AddRegistry(new DefaultRegistry());

                //Populate the container using the service collection
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
        }
    }
}
