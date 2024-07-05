using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly DBservices _dbServices;

        public CardsController()
        {
            _dbServices = new DBservices();
        }

        // GET: api/<CardsController>
        [HttpGet]
        public ActionResult<IEnumerable<Card>> Get()
        {
            var cards = _dbServices.ReadCards();
            if (cards == null || cards.Count == 0)
            {
                return NotFound("No cards found");
            }
            return Ok(cards);
        }

        // GET api/<CardsController>/5
        [HttpGet("{id}")]
        public ActionResult<Card> Get(int id)
        {
            var card = Card.GetCardById(id);
            if (card == null)
            {
                return NotFound("Card not found");
            }
            return Ok(card);
        }

        // PUT api/<CardsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Card card)
        {
            if (card == null || card.CardId != id)
            {
                return BadRequest("Invalid card data");
            }

            bool status = _dbServices.UpdateCard(card);
            if (!status)
            {
                return NotFound("Card not found");
            }
            return Ok("Card updated successfully");
        }

        // DELETE api/<CardsController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool status = _dbServices.DeleteCard(id);
            if (!status)
            {
                return NotFound("Card not found");
            }
            return Ok("Card deleted successfully");
        }
    }
}