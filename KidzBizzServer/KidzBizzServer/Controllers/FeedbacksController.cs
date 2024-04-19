using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KidzBizzServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        // GET: api/<FeedbacksController>
        [HttpGet]
        public IEnumerable<Feedback> Get()
        {
            Feedback feedback = new Feedback();
            return feedback.Read();
        }

        // GET api/<FeedbacksController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<FeedbacksController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<FeedbacksController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<FeedbacksController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
