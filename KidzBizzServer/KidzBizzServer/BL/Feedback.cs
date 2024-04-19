namespace KidzBizzServer.BL
{
    public class Feedback
    {
        int feedbackId;
        int userId;
        string description;
        int rating;

        public Feedback(int feedbackId, int userId, string description, int rating)
        {
            this.feedbackId = feedbackId;
            this.userId = userId;
            this.description = description;
            this.rating = rating;
        }

        public Feedback()
        {

        }

        public int FeedbackId { get => feedbackId; set => feedbackId = value; }
        public int UserId { get => userId; set => userId = value; }
        public string Description { get => description; set => description = value; }
        public int Rating { get => rating; set => rating = value; }

        public List<Feedback> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadFeedback();
        }

        public int Insert()
        {
            DBservices dbs = new DBservices();
            return dbs.InsertFeedback(this);
        }


    }
}
