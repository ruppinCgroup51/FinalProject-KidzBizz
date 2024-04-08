namespace KidzBizzServer.BL
{
    public class GameGuide
    {
        int gameGuideId;
        string title;
        string description;
        string content;

        public GameGuide(int gameGuideId, string title, string description, string content)
        {
            this.gameGuideId = gameGuideId;
            this.title = title;
            this.description = description;
            this.content = content;
        }

        public GameGuide()
        {

        }

        public int GameGuideId { get => gameGuideId; set => gameGuideId = value; }
        public string Title { get => title; set => title = value; }
        public string Description { get => description; set => description = value; }
        public string Content { get => content; set => content = value; }
    }
}
