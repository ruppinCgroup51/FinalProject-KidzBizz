using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
                var players = gameManagerWithAI.StartNewGame(user);
                return Ok(players);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error starting game: {ex.Message}");
            }
        }

        [HttpPost("rolldice")]
        public IActionResult RollDice([FromBody] Player player)
        {
            try
            {
                GameManagerWithAI gameManagerWithAI = new GameManagerWithAI();
                return Ok(gameManagerWithAI.RollDice(player));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error rolling dice: {ex.Message}");
            }
        }

        [HttpPost("payRent")]
        public IActionResult PayRent([FromBody] JsonElement jsonData)
        {
            try
            {
                int playerId = jsonData.GetProperty("playerId").GetInt32();
                int propertyOwnerId = jsonData.GetProperty("propertyOwnerId").GetInt32();
                int propertyId = jsonData.GetProperty("propertyId").GetInt32();

                GameManagerWithAI gameManagerWithAI = new GameManagerWithAI();
                gameManagerWithAI.PayRent(playerId, propertyOwnerId, propertyId);
                return Ok("Rent paid successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        // GET api/<GameManagerWithAIController>/5
        [HttpGet("GetById/{id}")]
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