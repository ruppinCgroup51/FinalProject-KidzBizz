using Microsoft.AspNetCore.Mvc;
using KidzBizzServer.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            User user = new User();
            return user.Read();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public int Post([FromBody] User user )
        {
            return user.Register();
        }

        // POST api/<UsersController>
        [HttpPost]
        [Route("login")]
        public ActionResult Login(User user)
        {

            User authenticatedUser = user.Login(user.Username, user.Password);

            if (authenticatedUser != null)
            {
                // Return the authenticated user
                return Ok(authenticatedUser);
            }
            else
            {
                // Return 404 if user does not exist or credentials are invalid
                return NotFound("User not found or invalid credentials");
            }
        }


        // PUT api/<UsersController>/5
        [HttpPut("update")]
        public User Put([FromBody] User user)
        {
            return user.Update();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("{username}")]
        public User Get(string username)
        {
            User user = new User();
            return user.ReadByUsername(username);
        }
    }
}
