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
using SP.Scheduler;

namespace SP.Scheduler.Services
{
    internal interface ISearchRobotServiceScopedProcessingService
    {
        void DoWork();
    }

    internal class SearchRobotServiceScopedProcessingService : CrolScopedProcessingService, ISearchRobotServiceScopedProcessingService
    {
        public SearchRobotServiceScopedProcessingService(DbContextOptions<DatabaseContext> Builder, IOptions<AppSettings> settings) : base(Builder, settings)
        {
        }

        List<string> KeywordExceptions = new List<string>();
        public void DoWork()
        {
            try
            {
              
            }
            catch (Exception ex)
            {
            }
        }
    }

}
