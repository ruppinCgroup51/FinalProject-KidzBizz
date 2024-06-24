//using KidzBizzServer.BL;
//using Microsoft.AspNetCore.Mvc;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace KidzBizzServer.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AnswersController : ControllerBase
//    {
//        // GET: api/<AnswersController>
//        [HttpGet]
//        public IEnumerable<Answer> Get()
//        {
//            Answer answer = new Answer();
//            return answer.Read();
//        }

//        // GET api/<AnswersController>/5
//        [HttpGet("{id}")]
//        public string Get(int id)
//        {
//            return "value";
//        }

//        // POST api/<AnswersController>
//        [HttpPost]

//        public IActionResult Post([FromBody] Answer answer)

//        {

//            try

//            {
//                if (answer == null)

//                {
//                    return BadRequest("Invalid data provided."); 
//                }

//                int result = answer.Insert(); // Attempt to insert the answer

//                return Ok(result); // Return the result of the insertion

//            }

//            catch (Exception ex)

//            {
//                return StatusCode(500, $"An error occurred: {ex.Message}"); 

//            }

//        }



//        // PUT api/<AnswersController>/5
//        [HttpPut("{id}")]
//        public void Put(int id, [FromBody] string value)
//        {
//        }

//        // DELETE api/<AnswersController>/5
//        [HttpDelete("{id}")]
//        public void Delete(int id)
//        {
//        }
//    }
//}
