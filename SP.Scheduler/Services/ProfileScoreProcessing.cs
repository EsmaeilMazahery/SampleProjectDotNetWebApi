using SP.DataLayer.Context;
using SP.DomainLayer.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SP.Infrastructure.Extensions;
using SP.DomainLayer.Models;
using SP.Infrastructure.Enumerations;

namespace SP.Scheduler.Services
{
    internal interface IProfileScoreScopedProcessingService
    {
        void DoWork();
    }

    internal class ProfileScoreScopedProcessingService : CrolScopedProcessingService, IProfileScoreScopedProcessingService
    {
        public ProfileScoreScopedProcessingService(DbContextOptions<DatabaseContext> Builder, IOptions<AppSettings> settings) : base(Builder, settings)
        {
        }

        public void DoWork()
        {
            try
            {
               

                //do some thing

            }
            catch (Exception ex)
            {
            }
        }
    }

}
