namespace KidzBizzServer.BL
{
    public class GameManagerWithAI
    {
        Player player = new Player();
        AIPlayer aiPlayer = new AIPlayer();
        int currentPlayerIndex;
        int diceRoll;

        public GameManagerWithAI(Player player, AIPlayer aiPlayer, int currentPlayerIndex, int diceRoll)
        {
            Player = player;
            AiPlayer = aiPlayer;
            CurrentPlayerIndex = currentPlayerIndex;
            DiceRoll = diceRoll;
        }

        public Player Player { get => player; set => player = value; }
        public AIPlayer AiPlayer { get => aiPlayer; set => aiPlayer = value; }
        public int CurrentPlayerIndex { get => currentPlayerIndex; set => currentPlayerIndex = value; }
        public int DiceRoll { get => diceRoll; set => diceRoll = value; }

        public void InitializeGame()
        {
            AddPlayers();  // Add players to the game
            ChooseFirstPlayer();  // Decide who starts first
            StartNewGame();  // Initialize the board and set starting positions
        }

        // פעולה להפעלת משחק חדש
        public void StartNewGame()
        {
            // לוגיקה להפעלת משחק חדש
           Game game = new Game();
            game.NumberOfPlayers = 2;
            game.GameDuration = new TimeSpan(0, 0, 0);
            game.GameStatus = "Active";
            game.GameTimestamp = DateTime.Now;

            // insert the new game into the data base inside table 'game'
            SaveDetailsGameToDatabase(game);

            // initial money for each player = 1500 
            // initial assets for each player = 0
            // initial position for each player = 0

            player.CurrentBalance = 1500;
            aiPlayer.CurrentBalance = 1500;
            player.CurrentPosition = 0;
            aiPlayer.CurrentPosition = 0;
            player.Properties = new List<Property>();
            aiPlayer.Properties = new List<Property>();



        }

        // פעולה שמגרילה מי השחקן שיתחיל ראשון
    

        // פעולה לשמירת פרטי משחק במסד הנתונים
        public void SaveDetailsGameToDatabase(Game game)
        {
          
            // לוגיקת שמירת פרטי משחק במסד הנתונים
        }

        // פעולה למעבר לתור השחקן הבא
        public void MoveToNextPlayer()
        {
            // לוגיקת מעבר לשחקן הבא
        }

        // פעולה לזריקת קובייה
        public void RollDice()
        {
            // לוגיקת זריקת קובייה
        }

        // פעולה לביצוע פעולה בהתאם לזריקת הקובייה
        public void PerformAction()
        {
            // לוגיקת ביצוע פעולה
        }

        // פעולה לסיום משחק
        public void EndGame()
        {
            // לוגיקת סיום משחק
        }

        // פעולה להפסקת משחק
        public void PauseGame()
        {
            // לוגיקת הפסקת משחק
        }

        // פעולה להמשך משחק
        public void ContinueGame()
        {
            // לוגיקת המשך משחק
        }   
    }
}
