using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        // GET: api/<PropertiesController>
        [HttpGet]
        public IEnumerable<Property> Get()
        {
            Property property = new Property();
            return property.Read();
          
        }

        [HttpGet]
        [Route("ReadPropertiesByPlayerId")]
        public IEnumerable<Property> Get(int id)
        {
           Property property = new Property();
            return property.ReadPropertiesByPlayerId(id);
        }
        // GET api/<PropertiesController>/CheckPropertyOwnership?propertyId=1&playerId=1&playerAiId=101
        [HttpGet]
        [Route("CheckPropertyOwnership")]
        public IActionResult CheckPropertyOwnership(int propertyId, int playerId, int playerAiId)
        {
            Property property = new Property();
            var owner = property.CheckPropertyOwnership(propertyId, playerId, playerAiId);
            if (owner != null)
            {
                return Ok(owner);
            }
            else
            {
                return NotFound("הנכס לא בבעלות של אף שחקן.");
            }
        }

        // POST api/<PropertiesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<PropertiesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PropertiesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
