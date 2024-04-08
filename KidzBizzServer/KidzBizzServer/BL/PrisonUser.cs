namespace KidzBizzServer.BL
{
    public class PrisonUser
    {
        int gameId;
        int playerId;
        string prisonStatus;

        public PrisonUser(int gameId, int playerId, string prisonStatus)
        {
            this.gameId = gameId;
            this.playerId = playerId;
            this.prisonStatus = prisonStatus;
        }

        public PrisonUser()
        {

        }

        public int GameId { get => gameId; set => gameId = value; }
        public int PlayerId { get => playerId; set => playerId = value; }
        public string PrisonStatus { get => prisonStatus; set => prisonStatus = value; }
    }
}
