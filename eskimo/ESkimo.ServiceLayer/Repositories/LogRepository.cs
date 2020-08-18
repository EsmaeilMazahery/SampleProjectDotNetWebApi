using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.Models;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace ESkimo.ServiceLayer.Services
{
    public interface ILogRepository : IGenericRepository<Log>
    {
        bool Insert(LogType type = LogType.Api_Generic, LogLevel level = LogLevel.ALL, string description = "", string logData = "");
    }

    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Log>> _logs;
        public static LogLevel LogLevel { set; get; } = LogLevel.WARN;
        public LogRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _logs = new Lazy<DbSet<Log>>(() => _uow.Set<Log>());
        }

        public bool Insert(LogType type = LogType.Api_Generic, LogLevel level = LogLevel.WARN, string description = "", string logData = "")
        {
            Log log = new Log()
            {
                dateTime = DateTime.Now,
                type = type,
                logData = logData,
                level = level,
                description = description
            };
            if (level >= LogLevel)
                ChangeState(log, EntityState.Added);
            SaveChanges();

            return true;
        }

    }
}




