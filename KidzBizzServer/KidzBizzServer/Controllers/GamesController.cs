using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        // GET: api/<GamesController>
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            Game game = new Game();
            return game.Read();
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GamesController>
        [HttpPost]
        public int Post([FromBody] Game game)
        {
            return game.InsertGame();
        }

        // PUT api/<GamesController>/5
        [HttpPut("update")]
        public Game Put([FromBody] Game game)
        {
            return game.UpdateGame();
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
