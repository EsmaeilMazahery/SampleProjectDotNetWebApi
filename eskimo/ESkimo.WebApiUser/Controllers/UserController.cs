using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ESkimo.DataLayer.Context;
using ESkimo.DomainLayer.ViewModel;
using ESkimo.Infrastructure.Enumerations;
using ESkimo.ServiceLayer.Services;
using ESkimo.WebApiUser.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESkimo.WebApiUser.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : BaseApiController
    {

        Lazy<IUserRepository> _userRepository;

        public UserController(IUnitOfWork uow, Lazy<IUserRepository> UserRepository) : base(uow)
        {
            _userRepository = UserRepository;
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Index(UserListViewModel model)
        {
            model = await _userRepository.Value.GetAllAsync(model);
            return Ok(model);
        }

        [HttpGet, Route("{Id}"), Authorize]
        public async Task<IActionResult> Get(int Id)
        {
            var model = await _userRepository.Value.GetAsync(Id);
            return Ok(model);
        }

        [HttpPost, Route(""), Authorize]
        public async Task<IActionResult> Insert([FromBody]InsertUserViewModel model)
        {
            try
            {
                var user = _userRepository.Value.Insert(new UserServiceModel
                {
                    email = model.email,
                    enable = model.enable,
                    family = model.family,
                    image = model.image,
                    mobile = model.mobile,
                    name = model.name,
                    password = model.password,
                    registerDate = DateTime.Now,
                    username = model.username,
                    selectedRoles = model.selectedRoles
                });
                await _uow.SaveAsync();

                return Ok(new { user.userId });

            }
            catch
            {
                return BadRequest();
            }          
        }

        [HttpPut, Route(""), Authorize]
        public async Task<IActionResult> Update([FromBody]UpdateUserViewModel model)
        {
            _userRepository.Value.Update(new UserServiceModel
            {
                userId=model.userId,
                email = model.email,
                enable = model.enable,
                family = model.family,
                image = model.image,
                mobile = model.mobile,
                name = model.name,
                password = model.password,
                registerDate = DateTime.Now,
                username = model.username,
                selectedRoles = model.selectedRoles
            });
            await _uow.SaveAsync();

            return NoContent();
        }

        [HttpDelete, Route("{Id}"), Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _userRepository.Value.DeleteAsync(Id);
            return NoContent();
        }

        [HttpGet, Route("filter"), Authorize]
        public async Task<IActionResult> Filter(string name, int[] regions = null, int? page = null, int? rowsPerPage = null)
        {
            var result = await _userRepository.Value.GetFilterAsync(name, regions);
            return Ok(new { result.list, result.allRows });
        }


    }
}