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

        [HttpPost("AddPropertyToPlayer")]
        public int AddPropertyToPlayer(int playerId, int propertyId)
        {
            Player player = new Player();
            return player.AddPropertyToPlayer(playerId, propertyId);
        }

        [HttpPost("GetPlayerProperties")]
        public List<Property> GetPlayerProperties(int playerId)
        {
            Player player = new Player();
            return player.GetPlayerProperties(playerId);
        }
      

        // GET api/<PlayersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PlayersController>
        [HttpPost]
        public int Post([FromBody] Player player)
        {
            return player.Insert();
        }

        [HttpPut("update")]
        public Player Put([FromBody] Player player)
        {
            return player.Update();
        }

        // DELETE api/<PlayersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
