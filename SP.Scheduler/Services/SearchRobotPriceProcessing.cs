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
    internal interface ISearchRobotPriceScopedProcessingService
    {
        void DoWork();
    }

    internal class SearchRobotPriceScopedProcessingService : CrolScopedProcessingService, ISearchRobotPriceScopedProcessingService
    {
        public SearchRobotPriceScopedProcessingService(DbContextOptions<DatabaseContext> Builder, IOptions<AppSettings> settings) : base(Builder, settings)
        {
        }

        List<string> KeywordExceptions = new List<string>();
        public void DoWork()
        {
            try
            {
                ExtensionMethods.DataTimeTryParseNullable(_settingRepository.Value.read(SettingType.LastCheckRobotPrice), out DateTime? lastCheckRobotPrice);

                KeywordExceptions = _uow.Set<KeywordException>().AsQueryable().Select(s => s.title).ToList();

                var newPrices = _priceRepository.Value.AsQueryable()
                    .Where(w => lastCheckRobotPrice == null || w.lastUpdate > lastCheckRobotPrice)
                    .Select(s => new
                    {
                        s.priceId,
                        s.serviceId,
                        s.title,
                        s.description,
                        s.group.name,
                        service = new
                        {
                            s.serviceId,
                            s.service.name,
                            s.service.serviceType,
                            businessType = s.service.businessId == null ? null : (Nullable<BusinessType>)s.service.business.businessType,
                            s.service.userId,
                            contactsServices = s.service.contactsServices.Count > 0,
                        },
                        discount = s.percentageDiscount > 0 && (s.dateFromDiscount == null || s.dateFromDiscount < DateTime.Now) && (s.dateToDiscount == null || s.dateToDiscount > DateTime.Now),
                        s.priceFrom,
                        s.priceTo,
                        s.priceType,
                        s.registerDate
                    }).ToListStep(100);


                foreach (var item in newPrices)
                {
                    List<KeywordScore> listKeywords = new List<KeywordScore>();

                    SchedulerExtensions.addListScore(_uow, listKeywords, item.title, 5, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.name, 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.service.name, 2, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, SchedulerExtensions.split100(item.description), 1, KeywordExceptions);

                    foreach (var keywordScore in listKeywords)
                    {
                        Keyword keyword = _uow.Set<Keyword>().Include(i => i.keywordPrices).FirstOrDefault(w => w.title == keywordScore.keyword);
                        if (keyword == null)
                        {
                            keyword = new Keyword
                            {
                                title = keywordScore.keyword,
                            };
                            _uow.Entry(keyword).State = EntityState.Added;
                        }

                        KeywordPrice keywordPrice = keyword.keywordPrices.FirstOrDefault(a => a.priceId == item.priceId);
                        if (keywordPrice == null)
                        {
                            keywordPrice = new KeywordPrice()
                            {
                                title = keywordScore.keyword,
                                score = keywordScore.score,
                                priceId = item.priceId,
                                keyword = keyword,
                                address = item.service.contactsServices,
                                discount = item.discount,
                                priceFrom = item.priceFrom,
                                priceTo = item.priceTo,
                                priceType = item.priceType,
                                registerDate = item.registerDate,
                                serviceType = item.service.serviceType,
                                userId = item.service.userId,
                                serviceId=item.serviceId
                            };

                            if (item.service.businessType.HasValue)
                                keywordPrice.businessType = item.service.businessType.Value;

                            _uow.Entry(keywordPrice).State = EntityState.Added;
                        }
                        else
                        {
                            keywordPrice.score = keywordScore.score;
                            keywordPrice.address = item.service.contactsServices;
                            keywordPrice.discount = item.discount;
                            keywordPrice.priceFrom = item.priceFrom;
                            keywordPrice.priceTo = item.priceTo;
                            keywordPrice.priceType = item.priceType;
                            keywordPrice.serviceType = item.service.serviceType;
                            keywordPrice.registerDate = item.registerDate;

                            if (item.service.businessType.HasValue)
                                keywordPrice.businessType = item.service.businessType.Value;

                            _uow.Entry(keywordPrice).State = EntityState.Modified;
                        }
                    }
                }

                _settingRepository.Value.write(SettingType.LastCheckRobotPrice, DateTime.Now.ToString());
                _uow.Save();
            }
            catch (Exception ex)
            {
            }
        }
    }

}
