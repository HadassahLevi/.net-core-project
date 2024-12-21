using Microsoft.AspNetCore.Mvc;
using music.Models;
using music.Services;
using music.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace music.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private IUsers UsersService;
        public UsersController(IUsers UsersService)
        {
            this.UsersService = UsersService;
        }

        [HttpGet]
        [Authorize(Policy="Admin")]
        public ActionResult<List<Users>> GetAll() =>
            UsersService.GetAll();


        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<Users> Get(int id)
        {
            var user = UsersService.Get(id);

            if (user == null)
                return NotFound();

            return user;
        }

        [HttpPost] 
        [Authorize(Policy = "Admin")]
        public IActionResult Create(Users user)
        {
            UsersService.Add(user);
            return CreatedAtAction(nameof(Create), new {id=user.Id}, user);

        }

        [HttpDelete("{id}")]
        [Authorize(Policy="Admin")]
        public IActionResult Delete(int id)
        {
            var user = UsersService.Get(id);
            if (user is null)
                return  NotFound();

            UsersService.Delete(id);

            return Content(UsersService.Count.ToString());
        }


        [HttpPost]
        [Route("/logIn")]
        public ActionResult<ObjectToReturn> LogIn([FromBody]Users user)
        {
            int userId = UsersService.ExistUser(user.Name ,user.Password);
            if(userId == -1)
                return Unauthorized();
            var claims = new List<Claim> { };
            if(user.Password.Equals("2955") && user.Name.Equals("דסי"))
                claims.Add(new Claim("type", "Admin"));
            else
                claims.Add(new Claim("type", "User"));
            
            claims.Add(new Claim("id", userId.ToString()));
            var token = TokenService.GetToken(claims);
            return new OkObjectResult(new { Id = userId ,Token = TokenService.WriteToken(token)});
        }
    }
}

public class ObjectToReturn
{
    public int Id { get; set; }
    public string Token { get; set; }
}
