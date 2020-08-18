using System;
using System.Linq;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.DomainLayer.ViewModel;
using SP.Infrastructure.Enumerations;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SP.ServiceLayer.Services
{
    public interface ISettingRepository : IGenericRepository<Setting>
    {
        string read(SettingType settingType);
        void write(SettingType settingType, string value);

        SettingCollection read(params SettingType[] settingTypes);
        void write(SettingCollection collection);
    }

    public class SettingRepository : GenericRepository<Setting>, ISettingRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Setting>> _setting;
        public SettingRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _setting = new Lazy<DbSet<Setting>>(() => _uow.Set<Setting>());
        }
        public string read(SettingType settingType)
        {
            return AsQueryable().Where(w => w.SettingType == settingType).FirstOrDefault()?.Value;
        }

        public SettingCollection read(params SettingType[] settingTypes)
        {
            SettingCollection settingCollection = new SettingCollection(AsQueryable().Where(w => settingTypes.Contains(w.SettingType)).ToList());
            return settingCollection;
        }

        public void write(SettingType settingType, string value)
        {
            Setting setting = AsQueryable().Where(w => w.SettingType == settingType).FirstOrDefault();
            if (setting == null)
            {
                setting = new Setting()
                {
                    Value = value,
                    SettingType = settingType
                };

                ChangeState(setting, EntityState.Added);
            }
            else
            {
                setting.Value = value;
                ChangeState(setting, EntityState.Modified);
            }
        }

        public void write(SettingCollection collection)
        {
            SettingType[] types = collection.Cast<SettingType>(c => c.SettingType);
            SettingCollection settingCollection = new SettingCollection(AsQueryable().AsNoTracking().Where(w => types.Contains(w.SettingType)).ToList());

            foreach (Setting item in collection)
            {
                if (settingCollection[item.SettingType] == null)
                {
                    Setting setting = new Setting()
                    {
                        Value = item.Value,
                        SettingType = item.SettingType
                    };

                    ChangeState(setting, EntityState.Added);
                }
                else if (settingCollection[item.SettingType].Value != item.Value && item.Value != null)
                {
                    ChangeState(item, EntityState.Modified);
                }
                else if (item.Value == null)
                {
                    ChangeState(settingCollection[item.SettingType], EntityState.Deleted);
                }
            }

        }
    }
}