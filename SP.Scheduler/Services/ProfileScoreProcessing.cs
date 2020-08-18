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

namespace SP.Scheduler.Services
{
    internal interface IProfileScoreScopedProcessingService
    {
        void DoWork();
    }

    internal class ProfileScoreScopedProcessingService : CrolScopedProcessingService, IProfileScoreScopedProcessingService
    {
        public ProfileScoreScopedProcessingService(DbContextOptions<DatabaseContext> Builder, IOptions<AppSettings> settings) : base(Builder, settings)
        {
        }

        public void DoWork()
        {
            try
            {
                var propertise = _propertiseRepository.Value.AsQueryable().Where(w => w.propertiseKey == Infrastructure.Enumerations.PropertiseKey.LastCheckProfileScore).FirstOrDefault();

                var newUsers = _userRepository.Value.AsQueryable()
                    .Where(w => w.lastUpdate > DateTime.Parse(propertise.value))
                    .Select(u => new
                    {
                        u.userId,
                        score = u.services.Sum(s => s.prices.Sum(p => p.medias.Count > 0 ? 1.5 : 1)) +
                         u.services.Sum(s => s.samples.Sum(sp => sp.medias.Count > 0 ? 2 : 1)) +
                         u.services.Sum(s => s.socials.Count) > 0 ? 1 : 0 +
                         u.services.Sum(s => s.portfolios.Count) > 0 ? 1 : 0 +
                         u.services.Sum(s => s.cataloges.Count) > 0 ? 1 : 0 +
                         u.services.Sum(s => s.website != "" ? 1 : 0) > 0 ? 1 : 0 +
                         u.services.Sum(s => s.description != "" ? 1 : 0) > 0 ? 1 : 0
                    }).ToListStep(100);


                foreach (var user in newUsers)
                {
                    _userRepository.Value.Update(w => w.userId == user.userId, u => new User() { searchScore = user.score });
                }
            }
            catch (Exception ex)
            {
            }
        }
    }

}
