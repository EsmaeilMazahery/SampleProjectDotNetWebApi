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
                ExtensionMethods.DataTimeTryParseNullable(_settingRepository.Value.read(SettingType.LastCheckRobotService), out DateTime? lastCheckRobotService);

                KeywordExceptions = _uow.Set<KeywordException>().AsQueryable().Select(s => s.title).ToList();

                var newServices = _serviceRepository.Value.AsQueryable()
                    .Where(w => lastCheckRobotService == null || w.lastUpdate > lastCheckRobotService)
                    .Select(s =>
                    new
                    {
                        s.serviceId,
                        s.user.searchScore,
                        s.businessId,
                        businessType = s.businessId == null ? null : (Nullable<BusinessType>)s.business.businessType,
                        s.serviceType,
                        s.registerDate,

                        s.name,
                        s.post,
                        s.unitOfWork,
                        s.website,
                        businessname = s.businessId == null ? null : s.business.name,
                        s.nameWorkPlace,
                        s.majorText,

                        s.description,
                        s.extraDescription,
                        locationsServices = s.locationsServices.Select(l => new { l.location.name, l.locationId }),
                        socials = s.socials.Where(w => w.type != SocialNetworkType.whatsapp).Select(so => so.address).ToArray(),
                        contactsServices = s.contactsServices.Count > 0,
                        s.userId,
                        cordinates = s.samples.Select(m => new { m.lat, m.lng, m.zoom }),

                        cataloges = s.cataloges.Select(c => c.title).ToArray(),
                        portfolios = s.portfolios.Select(p => p.title).ToArray(),

                        //countPrice = s.prices.Count,
                        prices = s.prices.Select(p => p.medias.Count > 0 ? 1.5 : 1),

                        //countSample = s.samples.Count,
                        samples = s.samples.Select(p => p.medias.Count > 0 ? 1.5 : 1),

                        //catalogesScore = s.cataloges.Select(c => c.title).ToArray().Length > 0 ? 1 : 0,
                        //portfoliosScore = s.portfolios.Select(p => p.title).ToArray().Length > 0 ? 1 : 0
                    }
                  ).ToListStep(100);

                //امتیاز کل
                const decimal MAXSCORE = 50;

                foreach (var item in newServices)
                {
                    double countPrice = item.prices.Count();
                    double countPriceImage = item.prices.Sum();

                    double countSample = item.samples.Count();
                    double countSampleImage = item.samples.Sum();


                    decimal priceScore = (decimal)(countPrice > 0 ? (5 *
                        (countPrice > 10 ? 1.2 : 1) *
                        (countPriceImage / countPrice + 1)) : 0);

                    decimal sampleScore = (decimal)(countSample > 0 ? (5 *
                      (countSample > 10 ? 1.2 : 1) *
                      (countSampleImage / countSample + 1)) : 0);

                    //جمع امتیاز ها
                    decimal rateScore = priceScore + sampleScore + (item.cataloges.Length > 0 ? 1 : 0) + (item.portfolios.Length > 0 ? 1 : 0);

                    List<KeywordScore> listKeywords = new List<KeywordScore>();

                    SchedulerExtensions.addListScore(_uow, listKeywords, item.name, (rateScore / MAXSCORE + 1) * 5, KeywordExceptions);
                    //SchedulerExtensions.addListScore(_uow, listKeywords, item.post, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.unitOfWork, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.website, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.businessname, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.nameWorkPlace, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.majorText, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);

                    SchedulerExtensions.addListScore(_uow, listKeywords, item.cataloges, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.portfolios, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.description, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.extraDescription, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.locationsServices.Select(s => s.name).ToArray(), (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.socials, (rateScore / MAXSCORE + 1) * 1, KeywordExceptions);

                    foreach (var keywordScore in listKeywords)
                    {
                        Keyword keyword = _uow.Set<Keyword>().Include(i => i.keywordServices)
                            .Select(s => new Keyword
                            {
                                title = s.title,
                                keywordServices = s.keywordServices.Select(ks => new KeywordService()
                                {
                                    keywordLoactions = ks.keywordLoactions,
                                    keywordCordinates = ks.keywordCordinates,
                                    address = ks.address,
                                    businessType = ks.businessType,
                                    keywordServiceId = ks.keywordServiceId,
                                    registerDate = ks.registerDate,
                                    score = ks.score,
                                    serviceId = ks.serviceId,
                                    serviceType = ks.serviceType,
                                    title = ks.title,
                                    userId = ks.userId
                                }).ToList()
                            })
                            .FirstOrDefault(w => w.title == keywordScore.keyword);

                        if (keyword == null)
                        {
                            keyword = new Keyword
                            {
                                title = keywordScore.keyword,
                            };
                            _uow.Entry(keyword).State = EntityState.Added;
                        }

                        KeywordService keywordService = keyword.keywordServices.FirstOrDefault(a => a.serviceId == item.serviceId);
                        if (keywordService == null)
                        {
                            keywordService = new KeywordService()
                            {
                                title = keywordScore.keyword,
                                score = keywordScore.score,
                                serviceId = item.serviceId,
                                keyword = keyword,
                                address = item.contactsServices,
                                serviceType = item.serviceType,
                                registerDate = item.registerDate,
                                userId = item.userId,

                            };

                            if (item.businessId.HasValue)
                                keywordService.businessType = item.businessType.Value;

                            _uow.Entry(keywordService).State = EntityState.Added;

                            foreach (var location in item.locationsServices)
                            {
                                var keywordLoaction = new KeywordLoaction()
                                {
                                    locationId = location.locationId,
                                    serviceId = item.serviceId,
                                    title = keywordScore.keyword,
                                    keywordService= keywordService
                                };
                                _uow.Entry(keywordLoaction).State = EntityState.Added;
                            }

                            foreach (var location in item.cordinates)
                            {
                                var keywordCordinate = new KeywordCordinate()
                                {
                                    lat = location.lat,
                                    lng = location.lng,
                                    zoom = location.zoom,
                                    serviceId = item.serviceId,
                                    title = keywordScore.keyword,
                                    keywordService = keywordService
                                };
                                _uow.Entry(keywordCordinate).State = EntityState.Added;
                            }
                        }
                        else
                        {
                            keywordService.score = keywordScore.score;
                            keywordService.address = item.contactsServices;
                            keywordService.serviceType = item.serviceType;
                            keywordService.registerDate = item.registerDate;
                            if (item.businessId.HasValue)
                                keywordService.businessType = item.businessType.Value;

                            _uow.Entry(keywordService).State = EntityState.Modified;

                            if (item.locationsServices != null)
                            {
                                var addedLoactions = item.locationsServices.Where(w => !keywordService.keywordLoactions.Any(a => a.locationId == w.locationId)).ToList();
                                var deletedLoactions = keywordService.keywordLoactions.Where(w => !item.locationsServices.Any(a => a.locationId == w.locationId)).ToList();
                                deletedLoactions.ForEach(x => _uow.Entry(x).State = EntityState.Deleted);
                                addedLoactions.ForEach(x => _uow.Entry(new KeywordLoaction()
                                {
                                    locationId = x.locationId,
                                    serviceId = item.serviceId,
                                    title = keywordScore.keyword,
                                    keywordServiceId = keywordService.keywordServiceId
                                }).State = EntityState.Added);
                            }

                            if (item.cordinates != null)
                            {
                                var addedCordinates = item.cordinates.Where(w => !keywordService.keywordCordinates.Any(a => a.lat == w.lat && a.lng == w.lng && a.zoom == w.zoom)).ToList();
                                var deletedCordinates = keywordService.keywordCordinates.Where(w => !item.cordinates.Any(a => a.lat == w.lat && a.lng == w.lng && a.zoom == w.zoom)).ToList();
                                deletedCordinates.ForEach(x => _uow.Entry(x).State = EntityState.Deleted);
                                addedCordinates.ForEach(x => _uow.Entry(new KeywordCordinate()
                                {
                                    lat = x.lat,
                                    lng = x.lng,
                                    zoom = x.zoom,
                                    serviceId = item.serviceId,
                                    title = keywordScore.keyword,
                                    keywordServiceId = keywordService.keywordServiceId
                                }).State = EntityState.Added);
                            }
                        }
                    }
                }

                _settingRepository.Value.write(SettingType.LastCheckRobotService, DateTime.Now.ToString());
                _uow.Save();
            }
            catch (Exception ex)
            {
            }
        }
    }

}
