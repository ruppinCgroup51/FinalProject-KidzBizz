using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameManagerWithAIController : ControllerBase
    {
        // GET: api/<GameManagerWithAIController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost("startnewgame")]
        public IActionResult StartNewGame(User user)
        {
            try
            {
                GameManagerWithAI gameManagerWithAI = new GameManagerWithAI();
                gameManagerWithAI.StartNewGame(user.UserId);
                return Ok("Game started successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error starting game: {ex.Message}");
            }
        }

        // GET api/<GameManagerWithAIController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GameManagerWithAIController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GameManagerWithAIController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GameManagerWithAIController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
