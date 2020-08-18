using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SP.DataLayer.Context;
using SP.DomainLayer.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace SP.Scheduler.Services
{
    internal class KeepAliveHostedService : IHostedService, IDisposable
    {
        public IUnitOfWork _uow;
        private Timer _timer;
        private AppSettings appSettings;
        public IServiceScopeFactory _serviceScopeFactory;

        private AppSettings GetAppSettings()
        {
            return appSettings;
        }

        private void SetAppSettings(AppSettings value)
        {
            appSettings = value;
        }

        public KeepAliveHostedService(IServiceScopeFactory serviceScopeFactory, IOptions<AppSettings> settings)
        {
            _serviceScopeFactory = serviceScopeFactory;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                DbContextOptions<DatabaseContext> Builder = scope.ServiceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>();

                _uow = new DatabaseContext(Builder);
                SetAppSettings(settings.Value);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                WebClient webClient = new WebClient();
                byte[] imgByte = webClient.DownloadData(appSettings.websiteScheduler + "/keep-alive?t=" + new Random().Next(0, 10000));
            }
            catch (Exception ex)
            {
            }
            try
            {
                WebClient webClient1 = new WebClient();
                byte[] imgByte1 = webClient1.DownloadData(appSettings.websiteClient + "/keep-alive?t=" + new Random().Next(0, 10000));
            }
            catch (Exception ex)
            {
            }
            try
            {
                WebClient webClient2 = new WebClient();
                byte[] imgByte2 = webClient2.DownloadData(appSettings.websiteUser + "/keep-alive?t=" + new Random().Next(0, 10000));
            }
            catch (Exception ex)
            {
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
