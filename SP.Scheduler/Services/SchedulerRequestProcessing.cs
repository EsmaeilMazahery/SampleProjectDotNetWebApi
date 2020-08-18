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

namespace SP.Scheduler.Services
{
    internal interface ISchedulerRequestScopedProcessingService
    {
        void DoWork();
    }

    internal class SchedulerRequestScopedProcessingService : CrolScopedProcessingService, ISchedulerRequestScopedProcessingService
    {
        public SchedulerRequestScopedProcessingService(DbContextOptions<DatabaseContext> Builder, IOptions<AppSettings> settings) : base(Builder, settings)
        {
        }

        public void DoWork()
        {
            try
            {
                //var list = _schedulerRequestRepository.Value.AsQueryable()
                //    .Where(w =>
                //    w.dateTime < DateTime.Now && w.expire>DateTime.Now &&
                //    w.result == Infrastructure.Enumerations.SchedulerRequestResult.ToDo).ToList();

                //foreach (var i in list)
                //{
                //    i.result = Infrastructure.Enumerations.SchedulerRequestResult.InProgress;
                //    _schedulerRequestRepository.Value.ChangeState(i, EntityState.Modified);
                //    _schedulerRequestRepository.Value.SaveChanges();
                //    if (i.type == Infrastructure.Enumerations.SchedulerRequestType.Consignment)
                //    {
                //    }
                //}
            }
            catch (Exception ex)
            {
            }
        }
    }

}
