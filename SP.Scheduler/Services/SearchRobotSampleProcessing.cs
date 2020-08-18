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
    internal interface ISearchRobotSampleScopedProcessingService
    {
        void DoWork();
    }

    internal class SearchRobotSampleScopedProcessingService : CrolScopedProcessingService, ISearchRobotSampleScopedProcessingService
    {
        public SearchRobotSampleScopedProcessingService(DbContextOptions<DatabaseContext> Builder, IOptions<AppSettings> settings) : base(Builder, settings)
        {
        }

        List<string> KeywordExceptions = new List<string>();
        public void DoWork()
        {
            try
            {
                ExtensionMethods.DataTimeTryParseNullable(_settingRepository.Value.read(SettingType.LastCheckRobotSample), out DateTime? lastCheckRobotSample);
                KeywordExceptions = _uow.Set<KeywordException>().AsQueryable().Select(s => s.title).ToList();

                var newSamples = _sampleRepository.Value.AsQueryable()
                    .Where(w => lastCheckRobotSample == null || w.lastUpdate > lastCheckRobotSample)
                    .Select(s => new
                    {
                        s.sampleId,
                        s.serviceId,
                        s.name,
                        s.description,
                        groupName = s.group.name,
                        serviceName = s.service.name,
                        projectName = s.project.name,
                        stateName = s.state.name,
                        cityName = s.city.name,
                        areaName = s.area.name,
                        service = new
                        {
                            s.serviceId,
                            s.service.name,
                            s.service.serviceType,
                            businessType = s.service.businessId == null ? null : (Nullable<BusinessType>)s.service.business.businessType,
                            s.service.userId
                        },
                        s.registerDate,
                        s.lat,
                        s.lng,
                        s.zoom,
                        
                    }).ToListStep(100);

                foreach (var item in newSamples)
                {
                    List<KeywordScore> listKeywords = new List<KeywordScore>();

                    SchedulerExtensions.addListScore(_uow, listKeywords, item.name, 5, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.service.name, 2, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.groupName, 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.projectName, 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.stateName, 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.cityName, 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, item.areaName, 1, KeywordExceptions);
                    SchedulerExtensions.addListScore(_uow, listKeywords, SchedulerExtensions.split100(item.description), 1, KeywordExceptions);

                    foreach (var keywordScore in listKeywords)
                    {
                        Keyword keyword = _uow.Set<Keyword>().Include(i => i.keywordSamples).FirstOrDefault(w => w.title == keywordScore.keyword);
                        if (keyword == null)
                        {
                            keyword = new Keyword
                            {
                                title = keywordScore.keyword,
                            };
                            _uow.Entry(keyword).State = EntityState.Added;
                        }

                        KeywordSample keywordSample = keyword.keywordSamples.FirstOrDefault(a => a.sampleId == item.sampleId);
                        if (keywordSample == null)
                        {
                            keywordSample = new KeywordSample()
                            {
                                title = keywordScore.keyword,
                                score = keywordScore.score,
                                sampleId = item.sampleId,
                                keyword = keyword,
                                serviceType = item.service.serviceType,
                                lat = item.lat,
                                lng = item.lng,
                                zoom = item.zoom,
                                registerDate = item.registerDate,
                                //locationId=item.
                                userId = item.service.userId,
                                serviceId=item.serviceId,
                            };

                            if (item.service.businessType.HasValue)
                                keywordSample.businessType = item.service.businessType.Value;

                            _uow.Entry(keywordSample).State = EntityState.Added;
                        }
                        else
                        {
                            keywordSample.score = keywordScore.score;
                            keywordSample.lat = item.lat;
                            keywordSample.lng = item.lng;
                            keywordSample.zoom = item.zoom;
                            keywordSample.serviceType = item.service.serviceType;
                            keywordSample.registerDate = item.registerDate;
                            if (item.service.businessType.HasValue)
                                keywordSample.businessType = item.service.businessType.Value;

                            _uow.Entry(keywordSample).State = EntityState.Modified;
                        }
                    }
                }

                _settingRepository.Value.write(SettingType.LastCheckRobotSample, DateTime.Now.ToString());
                _uow.Save();
            }
            catch (Exception ex)
            {
            }
        }
    }

}
