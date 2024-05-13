using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        // GET: api/<PlayersController>
        [HttpGet]
        public IEnumerable<Player> Get()
        {
            Player player = new Player();
            return player.Read();
        }

        // GET api/<PlayersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PlayersController>
        [HttpPost]
        public IActionResult Post([FromBody] Player player)
        {
            if (player == null)
            {
                return BadRequest("Invalid player data");
            }

            bool status = player.Insert();
            if (!status)
            {
                return Conflict("Player already exists");
            }
            return Ok("Player added successfully");
        }

        // PUT api/<PlayersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Player player)
        {
            if (player == null)
            {
                return BadRequest("Invalid player data");
            }

            bool status = player.UpdatePlayer(id);
            if (!status)
            {
                return NotFound("Player not found");
            }
            return Ok("Player updated successfully");
        }
        // DELETE api/<PlayersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
