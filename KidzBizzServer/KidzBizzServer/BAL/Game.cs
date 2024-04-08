namespace KidzBizzServer.BAL
{
    public class Game
    {

        int gameId;
        int numberOfPlayers;
        TimeSpan gameDuration;
        string gameStatus;
        DateTime gameTimestamp;

        public Game(int gameId, int numberOfPlayers, TimeSpan gameDuration, string gameStatus, DateTime gameTimestamp)
        {
            this.gameId = gameId;
            this.numberOfPlayers = numberOfPlayers;
            this.gameDuration = gameDuration;
            this.gameStatus = gameStatus;
            this.gameTimestamp = gameTimestamp;
        }

        public Game()
        {

        }

        public int GameId { get => gameId; set => gameId = value; }
        public int NumberOfPlayers { get => numberOfPlayers; set => numberOfPlayers = value; }
        public TimeSpan GameDuration { get => gameDuration; set => gameDuration = value; }
        public string GameStatus { get => gameStatus; set => gameStatus = value; }
        public DateTime GameTimestamp { get => gameTimestamp; set => gameTimestamp = value; }
    }

}
