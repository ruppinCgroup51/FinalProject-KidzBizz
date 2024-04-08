namespace KidzBizzServer.BL
{
    public class Question
    {
        int questionId;
        string questionText;

        public Question(int questionId, string questionText)
        {
            this.questionId = questionId;
            this.questionText = questionText;
        }

        public Question()
        {

        }

        public int QuestionId { get => questionId; set => questionId = value; }
        public string QuestionText { get => questionText; set => questionText = value; }
    }
}
