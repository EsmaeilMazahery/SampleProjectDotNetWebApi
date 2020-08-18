using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ESkimo.Infrastructure.Extensions;
using ESkimo.ServiceLayer.Services;
using ESkimo.Scheduler.Extension;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ESkimo.Scheduler.Services
{
    internal class KeepAliveHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private AppSettings appSettings;

        private void SetAppSettings(AppSettings value)
        {
            appSettings = value;
        }

        public KeepAliveHostedService(IOptions<AppSettings> settings)
        {
            SetAppSettings(settings.Value);
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
                try
                {
                    WebClient webClient = new WebClient();
                    byte[] imgByte = webClient.DownloadData(appSettings.websiteScheduler + "/keep-alive?t=" + new Random().Next(0, 10000));
                }
                catch { }
                try
                {
                    WebClient webClient1 = new WebClient();
                    byte[] imgByte1 = webClient1.DownloadData(appSettings.websiteMember + "/keep-alive?t=" + new Random().Next(0, 10000));
                }
                catch { }
                try
                {
                    WebClient webClient2 = new WebClient();
                    byte[] imgByte2 = webClient2.DownloadData(appSettings.websiteUser + "/keep-alive?t=" + new Random().Next(0, 10000));
                }
                catch { }
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
