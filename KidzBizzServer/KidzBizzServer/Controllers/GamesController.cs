using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        // GET: api/<GamesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<GamesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GamesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GamesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GamesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost("startGameWithAI")]
        public IActionResult StartGame(string username)
        {
            Game game = new Game
            {
                NumberOfPlayers = 2,
                GameDuration = new TimeSpan(0),
                GameStatus = "Started",
                GameTimestamp = DateTime.Now
            };

            // יצירת משחק חדש בדאטה בייס 

            DBservices dbs = new DBservices();
            // Insert the game to the database
            game.GameId = dbs.InsertGame(game);
            
            // קבלת המידע על המשתמשים שלי 
            // Human user
            User user = new User();
            User humanUser = user.ReadByUsername(username);

            // AI user
            User aiUser = user.ReadByUsername("AI");

            //יצירת השחקנים של המשחק 
            // Create players
            Player humanPlayer = new Player { User = humanUser, CurrentPosition = 0, CurrentBalance = 1500, PlayerStatus = "Active" };
            Player aiPlayer = new Player { User = aiUser, CurrentPosition = 0, CurrentBalance = 1500, PlayerStatus = "Active" };

            //// Insert players to the database
            //InsertPlayer(humanPlayer, game.GameId);
            //InsertPlayer(aiPlayer, game.GameId);

            // Return the game and the players
            return Ok(new { Game = game, Players = new[] { humanPlayer, aiPlayer } });
        }

    }
}
