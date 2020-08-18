using SP.Scheduler.Services;
using SP.DomainLayer.ViewModel;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SP.Scheduler.Services
{
    public class HostServiceHelper : IHostedService
    {
        public IServiceProvider Services { get; }
        private AppSettings AppSettings { get; set; }
        private IHostingEnvironment env { get; set; }
        public HostServiceHelper(IServiceProvider services, IOptions<AppSettings> settings, IHostingEnvironment env)
        {
            Services = services;
            AppSettings = settings.Value;
            this.env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                if (AppSettings.ActiveSchedulerRequest)
                {
                    Task.Run(() => SchedulerRequest(), cancellationToken);
                }

                if (AppSettings.ActiveSearchRobotService)
                {
                    Task.Run(() => SearchRobotService(), cancellationToken);
                }

            }, cancellationToken);
        }

        private void SchedulerRequest()
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<ISchedulerRequestScopedProcessingService>();

                while (true)
                {
                    scopedProcessingService.DoWork();
                    Thread.Sleep(new TimeSpan(0, 1, 0));
                }
            }
        }

        private void SearchRobotService()
        {
            using (var scope = Services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<ISearchRobotServiceScopedProcessingService>();

                while (true)
                {
                    scopedProcessingService.DoWork();
                    Thread.Sleep(new TimeSpan(0, 1, 0));
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //Your logical
            throw new NotImplementedException();
        }
    }
}
