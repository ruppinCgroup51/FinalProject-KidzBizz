namespace KidzBizzServer.BL
{
    public class Game
    {

        int gameId;
        int numberOfPlayers;
        string gameDuration;
        string gameStatus;
        DateTime gameTimestamp;

        public Game(int gameId, int numberOfPlayers, string gameDuration, string gameStatus, DateTime gameTimestamp)
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
        public string GameDuration { get => gameDuration; set => gameDuration = value; }
        public string GameStatus { get => gameStatus; set => gameStatus = value; }
        public DateTime GameTimestamp { get => gameTimestamp; set => gameTimestamp = value; }

        public int InsertGame()
        {
            DBservices dbs = new DBservices();
            return dbs.InsertGame(this);
        }

        public Game UpdateGame()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateGame(this);
        }

        public List<Game> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadGames();
        }

        public Game GetById(int gameId)
        {
            DBservices dbs = new DBservices();
            return dbs.GetGameById(gameId);

        }
    }

}
