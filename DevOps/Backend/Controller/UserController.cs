using Backend.Models;
using Backend.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controller
{
    [Authorize]
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class UserController(IDataRepository<Users> usersRepo) : ControllerBase
    {
        private readonly IDataRepository<Users> _userRepo = usersRepo;


        //Team Leader API
        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            return new JsonResult(Ok(await _userRepo.GetAllDevsAsync()));
        }



    }

}