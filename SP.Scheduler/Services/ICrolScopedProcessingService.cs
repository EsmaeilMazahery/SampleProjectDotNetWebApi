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
        public Lazy<IUserRepository> _userRepository;
        public Lazy<IServiceRepository> _serviceRepository;
        public Lazy<ISampleRepository> _sampleRepository;
        public Lazy<IPriceRepository> _priceRepository;
        public Lazy<IPropertiseRepository> _propertiseRepository;
        public Lazy<ISettingRepository> _settingRepository;

        public CrolScopedProcessingService(DbContextOptions<DatabaseContext> Builder, IOptions<AppSettings> settings)
        {
            AppSettings = settings.Value;

            _uow = new DatabaseContext(Builder);
            _serviceRepository = new Lazy<IServiceRepository>(() => new ServiceRepository(_uow));
            _sampleRepository = new Lazy<ISampleRepository>(() => new SampleRepository(_uow));
            _priceRepository = new Lazy<IPriceRepository>(() => new PriceRepository(_uow));
            _propertiseRepository = new Lazy<IPropertiseRepository>(() => new PropertiseRepository(_uow));
            _userRepository = new Lazy<IUserRepository>(() => new MemberRepository(_uow));
            _settingRepository = new Lazy<ISettingRepository>(() => new SettingRepository(_uow));
        }
    }
}
