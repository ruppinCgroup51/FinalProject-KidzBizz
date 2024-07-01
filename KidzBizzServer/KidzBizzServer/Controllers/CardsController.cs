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
            var cards = _dbServices.ReadCards();
            var card = cards.FirstOrDefault(c => c.CardId == id);
            if (card == null)
            {
                return NotFound("Card not found");
            }
            return Ok(card);
        }

        // POST api/<CardsController>
        [HttpPost]
        public IActionResult Post([FromBody] Card card)
        {
            if (card == null)
            {
                return BadRequest("Invalid card data");
            }

            bool status = card.UpdateCard();
            if (!status)
            {
                return StatusCode(500, "An error occurred while creating the card");
            }
            return CreatedAtAction(nameof(Get), new { id = card.CardId }, card);
        }

        // PUT api/<CardsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Card card)
        {
            if (card == null || card.CardId != id)
            {
                return BadRequest("Invalid card data");
            }

            bool status = card.UpdateCard();
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
            // מחיקה לא מיושמת בקוד זה, יש להוסיף אם נדרש
            return StatusCode(501, "Not implemented");
        }
    }
}