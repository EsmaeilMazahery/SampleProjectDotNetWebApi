using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace ESkimo.WebApiUser.Controllers
{
    [Produces("application/json")]
    [Route("api/pocketPost")]
    public class PocketPostController : BaseApiController
    {
        Lazy<IPocketPostRepository> _pocketPostRepository;
        Lazy<IFactorRepository> _factorRepository;

        public PocketPostController(IUnitOfWork uow,
            Lazy<IPocketPostRepository> PocketPostRepository,
            Lazy<IFactorRepository> FactorRepository) : base(uow)
        {
            _pocketPostRepository = PocketPostRepository;
            _factorRepository = FactorRepository;
        }

        [HttpGet, Route(""), Authorize]
        public async Task<IActionResult> Index(PocketPostListViewModel model)
        {
            model = await _pocketPostRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _pocketPostRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertPocketPostViewModel model)
        {
            try
            {
                var pocketPost = _pocketPostRepository.Value.Insert(new PocketPostServiceModel
                {
                    amount = model.amount,
                    sendDateTime = model.sendDateTime,
                    description = model.description,
                    userId = userId.Value,
                    userSenderId = model.userSenderId,
                    selectedFactors= model.selectedFactors
                });
                await _uow.SaveAsync();

                _factorRepository.Value.Update(w => model.selectedFactors.Contains(w.factorId),
                    u => new DomainLayer.Models.Factor() { pocketPostId = pocketPost.pocketPostId });

                await _uow.SaveAsync();

                return Ok(new { pocketPost.pocketPostId });

            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _pocketPostRepository.Value.DeleteAsync(Id);
            return NoContent();
        }
    }
}