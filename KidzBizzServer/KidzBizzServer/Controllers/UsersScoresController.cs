using Microsoft.AspNetCore.Mvc;
using KidzBizzServer.BL;
using Microsoft.Extensions.Configuration.UserSecrets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersScoresController : ControllerBase
    {
        // GET: api/<UsersScoresController>
        [HttpGet]
        public IEnumerable<UserScore> Get()
        {
            UserScore userScore = new UserScore();
            return userScore.Read();
        }

        // GET api/<UsersScoresController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersScoresController>
        [HttpPost]
        public int Post([FromBody] UserScore userScore)
        {
            return userScore.addUserScore();

        }

        // PUT api/<UsersScoresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersScoresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
