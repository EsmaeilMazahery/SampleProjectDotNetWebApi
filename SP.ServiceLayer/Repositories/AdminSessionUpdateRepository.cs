﻿using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SP.ServiceLayer.Services
{
    public interface IUserSessionUpdateRepository : IGenericRepository<UserSessionUpdate>
    {
        void AddUserSessionUpdate(int userId);
        int ReadMaxUserSessionUpdateId();
    }

    public class UserSessionUpdateRepository : GenericRepository<UserSessionUpdate>, IUserSessionUpdateRepository
    {
        Lazy<DbSet<UserSessionUpdate>> _userSessionUpdates;
        public UserSessionUpdateRepository(IUnitOfWork uow) : base(uow)
        {
            _userSessionUpdates = new Lazy<DbSet<UserSessionUpdate>>(() => uow.Set<UserSessionUpdate>());
        }

        /// <summary>
        /// رکورد جدید اضافه میکند و استیت را اَدِد میکند
        /// </summary>
        public void AddUserSessionUpdate(int adminId)
        {
            UserSessionUpdate record = new UserSessionUpdate
            {
                userId = adminId
            };
            ChangeState(record, EntityState.Added);
        }

        public int ReadMaxUserSessionUpdateId()
        {
            int? max = _userSessionUpdates.Value.Max(m => (int?)m.userSessionUpdateId);
            if (max == null)
                return 0;
            return max.Value;
        }
    }
}
