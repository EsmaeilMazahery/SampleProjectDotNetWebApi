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

namespace ESkimo.ServiceLayer.Services
{
    public interface ISettingRepository : IGenericRepository<Setting>
    {
        Task<string> read(SettingType settingType);
        Task write(SettingType settingType, string value);

        Task<SettingCollection> read(params SettingType[] settingTypes);
        Task write(SettingCollection collection);
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

        public Task<string> read(SettingType settingType)
        {
            return AsQueryable().Where(w => w.SettingType == settingType).FirstOrDefaultAsync().ContinueWith(setting => setting.Result?.Value??"");
        }

        public Task<SettingCollection> read(params SettingType[] settingTypes)
        {
            return AsQueryable().Where(w => settingTypes.Contains(w.SettingType)).ToListAsync().ContinueWith(settings =>
             {
                 SettingCollection settingCollection = new SettingCollection(settings.Result);
                 return settingCollection;
             });
        }

        public Task write(SettingType settingType, string value)
        {
            return AsQueryable().Where(w => w.SettingType == settingType).FirstOrDefaultAsync().ContinueWith(
                setting =>
                {

                    if (setting.Result == null)
                    {
                        var settingEntity = new Setting()
                        {
                            Value = value,
                            SettingType = settingType
                        };

                        ChangeState(settingEntity, EntityState.Added);
                    }
                    else
                    {
                        setting.Result.Value = value;
                        ChangeState(setting.Result, EntityState.Modified);
                    }
                }
                );
        }

        public Task write(SettingCollection collection)
        {
            SettingType[] types = collection.Cast<SettingType>(c => c.SettingType);

            return AsQueryable().AsNoTracking()
                              .Where(w => types.Contains(w.SettingType)).ToListAsync().ContinueWith(
                  list =>
                  {
                      SettingCollection settingCollection = new SettingCollection(list.Result);

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

                  });
        }
    }












}




