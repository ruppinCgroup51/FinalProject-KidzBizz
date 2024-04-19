namespace KidzBizzServer.BL
{
    public class Answer
    {
        int answerId;
        int questionId;
        string answerText;
        bool isCorrect;

        public Answer(int answerId, int questionId, string answerText, bool isCorrect)
        {
            this.answerId = answerId;
            this.questionId = questionId;
            this.answerText = answerText;
            this.isCorrect = isCorrect;
        }
    
        public Answer()
        {

        }

        public int AnswerId { get => answerId; set => answerId = value; }
        public int QuestionId { get => questionId; set => questionId = value; }
        public string AnswerText { get => answerText; set => answerText = value; }
        public bool IsCorrect { get => isCorrect; set => isCorrect = value; }

        public int Insert()
        {
            DBservices dbs = new DBservices();
            return dbs.InsertAnswer(this);

        }
    }
}
