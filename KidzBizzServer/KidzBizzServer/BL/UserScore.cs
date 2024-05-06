namespace KidzBizzServer.BL
{
    public class UserScore
    {
        int score;
        DateTime lastUpdated;
        int userId;
        string username;

    
        public UserScore()
        {

        }
        public UserScore(int score, DateTime lastUpdated, string username, int userId)
        {
            this.score = score;
            this.lastUpdated = lastUpdated;
            this.username = username;
            this.userId = userId;
        }

        public int Score { get => Score; set => Score = value; }
        public DateTime LastUpdated1 { get => lastUpdated; set => lastUpdated = value; }
        public string Username { get => username; set => username = value; }
        public int UserId { get => userId; set => userId = value; }

        public List<UserScore> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadUsersScores();
        }

        public int addUserScore()
        {
            DBservices dbs = new DBservices();
           return dbs.addUserScore(this);
        }


    }
}
