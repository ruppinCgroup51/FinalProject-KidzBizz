using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private DBservices dbServices = new DBservices();

        // GET: api/<PlayersController>
        [HttpGet]
        public IEnumerable<Player> Get()
        {
            return dbServices.ReadPlayers();
        }

        // GET api/<PlayersController>/5
        [HttpGet("{id}")]
        public ActionResult<Player> Get(int id)
        {
            Player player = dbServices.ReadPlayerById(id);
            if (player != null)
            {
                return player;
            }
            else
            {
                return NotFound();
            }
        }

        // POST api/<PlayersController>
        [HttpPost]
        public ActionResult Post([FromBody] Player player)
        {
            int result = dbServices.InsertPlayer(player);
            if (result > 0)
            {
                return CreatedAtAction(nameof(Get), new { id = player.PlayerId }, player);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT api/<PlayersController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Player player)
        {
            if (id != player.PlayerId)
            {
                return BadRequest();
            }
            Player updatedPlayer = dbServices.UpdatePlayer(player);
            if (updatedPlayer != null)
            {
                return Ok(updatedPlayer);
            }
            else
            {
                return NotFound();
            }
        }

        // DELETE api/<PlayersController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool result = dbServices.DeletePlayer(id);
            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
