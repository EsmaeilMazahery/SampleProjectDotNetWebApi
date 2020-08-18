using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ESkimo.Scheduler.Services
{
    public class HostServiceHelper : IHostedService
    {
        public IServiceProvider Services { get; }
        public HostServiceHelper(IServiceProvider services)
        {
            Services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {

            }, cancellationToken);
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            //Your logical
            throw new NotImplementedException();
        }
    }
}
