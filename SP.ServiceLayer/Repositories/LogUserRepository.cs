using System;
using System.Linq;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.Infrastructure.Enumerations;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SP.ServiceLayer.Services
{
    public interface ILogUserRepository : IGenericRepository<LogUser>
    {
        void AddLog(int userId, LogUserType type, DateTime? dateTime = null, string Description = "", bool save = true, dynamic Model = null);
    }

    public class LogUserRepository : GenericRepository<LogUser>, ILogUserRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<LogUser>> _logUsers;
        public LogUserRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _logUsers = new Lazy<DbSet<LogUser>>(() => _uow.Set<LogUser>());
        }

        public void AddLog(int userId, LogUserType type, DateTime? dateTime=null, string description = "",bool save=true, dynamic Model = null)
        {
            if (!dateTime.HasValue)
                dateTime = DateTime.Now;

            LogUser logUser = new LogUser()
            {
                dateTime = dateTime.Value,
                description =string.IsNullOrEmpty(description) ? JsonConvert.SerializeObject(Model): description,
                type = type,
                userId = userId,
            };

            ChangeState(logUser, EntityState.Added);

            if (save)
                _uow.Save();
        }
    }
}
