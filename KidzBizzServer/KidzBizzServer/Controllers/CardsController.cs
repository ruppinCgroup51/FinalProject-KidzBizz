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
            var cards = Card.GetAllCards();
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

        // GET: api/<CardsController>/command
        [HttpGet("command")]
        public ActionResult<IEnumerable<CommandCard>> GetCommandCards()
        {
            var cards = Card.GetAllCommandCards();
            if (cards == null || cards.Count == 0)
            {
                return NotFound("No command cards found");
            }
            return Ok(cards);
        }

        // GET: api/<CardsController>/surprise
        [HttpGet("surprise")]
        public ActionResult<IEnumerable<SurpriseCard>> GetSurpriseCards()
        {
            var cards = Card.GetAllSurpriseCards();
            if (cards == null || cards.Count == 0)
            {
                return NotFound("No surprise cards found");
            }
            return Ok(cards);
        }

        // GET: api/<CardsController>/didyouknow
        [HttpGet("didyouknow")]
        public ActionResult<IEnumerable<DidYouKnowCard>> GetDidYouKnowCards()
        {
            var cards = Card.GetAllDidYouKnowCards();
            if (cards == null || cards.Count == 0)
            {
                return NotFound("No did you know cards found");
            }
            return Ok(cards);
        }

        // POST api/<CardsController>
        [HttpPost]
        public IActionResult Post([FromBody] Card card)
        {
            if (card == null)
            {
                return BadRequest("Invalid card data");
            }

            bool status = _dbServices.InsertCard(card);
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