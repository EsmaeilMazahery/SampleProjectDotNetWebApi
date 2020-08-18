using System;
using System.Collections.Generic;
using System.Linq;
using SP.DataLayer.Context;
using SP.DomainLayer.Models;
using SP.DomainLayer.ViewModel;
using SP.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace SP.ServiceLayer.Services
{
    public interface IMediaRepository : IGenericRepository<Media>
    {
        Media Register(Media model);
        void Delete(int mediaId);
    }

    public class MediaRepository : GenericRepository<Media>, IMediaRepository
    {
        IUnitOfWork _uow;
        Lazy<DbSet<Media>> _medias;
        public MediaRepository(IUnitOfWork uow) : base(uow)
        {
            _uow = uow;
            _medias = new Lazy<DbSet<Media>>(() => _uow.Set<Media>());
        }
        
        public Media Register(Media model)
        {
            Media media = new Media()
            {
                address = model.address,
                title = model.title,
                date = DateTime.Now,
            };
            var emedia = _uow.AddEntity(media);

            return emedia.Entity;
        }

        public void Delete(int mediaId)
        {
            base.Delete(d => d.mediaId == mediaId);
        }
    }
}
