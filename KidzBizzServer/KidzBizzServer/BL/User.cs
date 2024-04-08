namespace KidzBizzServer.BL
{
    public class User
    {
        int userId;
        string username;
        string password;
        string firstName;
        string lastName;
        string avatarPicture;
        DateTime dateOfBirth;
        string gender;

        public User(int userId, string username, string password, string firstName, string lastName, string avatarPicture, DateTime dateOfBirth, string gender)
        {
            this.userId = userId;
            this.username = username;
            this.password = password;
            this.firstName = firstName;
            this.lastName = lastName;
            this.avatarPicture = avatarPicture;
            this.dateOfBirth = dateOfBirth;
            this.gender = gender;
        }

        public User()
        {

        }

        public int UserId { get => userId; set => userId = value; }
        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string AvatarPicture { get => avatarPicture; set => avatarPicture = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Gender { get => gender; set => gender = value; }
    }
}
