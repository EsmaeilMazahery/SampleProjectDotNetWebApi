using SP.DataLayer.Context;
using SP.DomainLayer.ViewModel;
using SP.ServiceLayer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SP.Scheduler.Services
{
    public class CrolScopedProcessingService
    {
        private AppSettings AppSettings { get; set; }

        public IUnitOfWork _uow;
        public Lazy<IMemberRepository> _memberRepository;
        public Lazy<IServiceRepository> _serviceRepository;
        public Lazy<IPropertiseRepository> _propertiseRepository;
        public Lazy<ISettingRepository> _settingRepository;

        public CrolScopedProcessingService(DbContextOptions<DatabaseContext> Builder, IOptions<AppSettings> settings)
        {
            AppSettings = settings.Value;

            _uow = new DatabaseContext(Builder);
            _serviceRepository = new Lazy<IServiceRepository>(() => new ServiceRepository(_uow));
            _propertiseRepository = new Lazy<IPropertiseRepository>(() => new PropertiseRepository(_uow));
            _memberRepository = new Lazy<IMemberRepository>(() => new MemberRepository(_uow));
            _settingRepository = new Lazy<ISettingRepository>(() => new SettingRepository(_uow));
        }
    }
}
