using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SP.DataLayer.Context;
using SP.DomainLayer.ViewModel;
using SP.Scheduler.Services;
using SP.ServiceLayer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SP.Scheduler
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

            services.Configure<AppSettings>(Configuration.GetSection("AppSettingsScheduler"));

            services.AddControllers();

            ConfigureIoC(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
            }

            //uow.initdb();
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
            services.AddHostedService<KeepAliveHostedService>();
            services.AddHostedService<HostServiceHelper>();
            services.AddScoped<ISchedulerRequestScopedProcessingService, SchedulerRequestScopedProcessingService>();

            services.AddScoped<ISearchRobotServiceScopedProcessingService, SearchRobotServiceScopedProcessingService>();
            services.AddScoped<ISearchRobotSampleScopedProcessingService, SearchRobotSampleScopedProcessingService>();
            services.AddScoped<ISearchRobotPriceScopedProcessingService, SearchRobotPriceScopedProcessingService>();



            //services.AddScoped<IUnitOfWork, DatabaseContext>(provider => new DatabaseContext(provider.GetService<DbContextOptions<DatabaseContext>>()));
            //services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));
            //services.AddTransient<IAreaRepository, AreaRepository>();
            //services.AddTransient<IAdminRepository, AdminRepository>();
            //services.AddTransient<IBookmarkRepository, BookmarkRepository>();
            //services.AddTransient<ICallRepository, CallRepository>();
            //services.AddTransient<IBusinessRepository, BusinessRepository>();
            //services.AddTransient<IContactRepository, ContactRepository>();
            //services.AddTransient<IInfoVerifyRepository, InfoVerifyRepository>();
            //services.AddTransient<ILocationRepository, LocationRepository>();
            //services.AddTransient<IMediaRepository, MediaRepository>();
            //services.AddTransient<IPriceRepository, PriceRepository>();
            //services.AddTransient<IPriceGroupRepository, PriceGroupRepository>();
            //services.AddTransient<IProjectRepository, ProjectRepository>();
            //services.AddTransient<IRoleRepository, RoleRepository>();
            //services.AddTransient<ISampleRepository, SampleRepository>();
            //services.AddTransient<ISampleGroupRepository, SampleGroupRepository>();
            //services.AddTransient<IServiceRepository, ServiceRepository>();
            //services.AddTransient<ISettingRepository, SettingRepository>();
            //services.AddTransient<ISiteRepository, SiteRepository>();
            //services.AddTransient<ISmsRepository, SmsRepository>();
            //services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<IUserVisitRepository, UserVisitRepository>();
            //services.AddTransient<IMemberVisitRepository, MemberVisitRepository>();
            //services.AddTransient<IRep_DailyChartRepository, Rep_DailyChartRepository>();

            //services.AddTransient<ILogUserRepository, LogUserRepository>();
            //services.AddTransient<IKeywordRepository, KeywordRepository>();
            //services.AddTransient<IKeywordServiceRepository, KeywordServiceRepository>();
            //services.AddTransient<IKeywordSampleRepository, KeywordSampleRepository>();
            //services.AddTransient<IKeywordPriceRepository, KeywordPriceRepository>();
            //services.AddTransient<IKeywordCordinateRepository, KeywordCordinateRepository>();
            //services.AddTransient<IKeywordLoactionRepository, KeywordLoactionRepository>();
            //services.AddTransient<IKeywordExceptionRepository, KeywordExceptionRepository>();
            //services.AddTransient<IKeywordTranslateRepository, KeywordTranslateRepository>();
            //services.AddTransient<ICommentRepository, CommentRepository>();
            //services.AddTransient<ILikeRepository, LikeRepository>();
            //services.AddTransient<ISiteRepository, SiteRepository>();
            //services.AddTransient<ISiteRepository, SiteRepository>();

            //var container = new Container();

            //container.Configure(config =>
            //{
            //    config.AddRegistry(new DefaultRegistry());

            //    //Populate the container using the service collection
            //    config.Populate(services);
            //});

            //return container.GetInstance<IServiceProvider>();
        }
    }
}
