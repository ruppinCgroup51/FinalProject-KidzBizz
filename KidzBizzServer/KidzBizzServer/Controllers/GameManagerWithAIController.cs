using KidzBizzServer.BL;
using Microsoft.AspNetCore.Mvc;

namespace KidzBizzServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameManagerWithAIController : ControllerBase
    {
        private readonly GameManagerWithAI _gameManager;

        public GameManagerWithAIController()
        {
            _gameManager = new GameManagerWithAI();
        }

        [HttpPost("start")]
        public IActionResult StartNewGame([FromBody] User user)
        {
            var players = _gameManager.StartNewGame(user);
            return Ok(players);
        }

        [HttpPost("end")]
        public IActionResult EndGame()
        {
            _gameManager.EndGame();
            return Ok("Game Ended");
        }

        [HttpPost("roll-dice")]
        public IActionResult RollDice([FromBody] Player player)
        {
            var updatedPlayer = _gameManager.RollDice(player);
            return Ok(updatedPlayer);
        }

        [HttpPost("pay-rent")]
        public IActionResult PayRent(int playerId, int propertyOwnerId, int propertyId)
        {
            _gameManager.PayRent(playerId, propertyOwnerId, propertyId);
            return Ok("Rent Paid");
        }

        [HttpPost("pause")]
        public IActionResult PauseGame()
        {
            _gameManager.PauseGame();
            return Ok("Game Paused");
        }

        [HttpPost("continue")]
        public IActionResult ContinueGame()
        {
            _gameManager.ContinueGame();
            return Ok("Game Continued");
        }
    }
}
