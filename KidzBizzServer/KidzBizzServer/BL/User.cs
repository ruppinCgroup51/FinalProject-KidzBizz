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

        //User Registration
        public int Register()
        {
            DBservices dbs = new DBservices();

            List<User> users = Read();

            foreach (var user in users)
            {
                if (user.username == this.username) { return -1; }
            }

            return dbs.RegisterUser(this);
        }

        public List<User> Read()
        {
            DBservices dbs = new DBservices();
            return dbs.ReadUsers();
        }

        public User Update()
        {
            DBservices dbs = new DBservices();
            return dbs.UpdateUser(this);

        }

        // login user
        public User Login(string username, string password)
        {
            DBservices dbs = new DBservices();
            return dbs.LoginUser(username, password);

        }

        
        public User ReadByUsername(string username)
        {
            DBservices dbs = new DBservices();
            return dbs.ReadUserByUsername(username);
        }

        ////delete user
        //public int Delete(string email)
        //{
        //    DBservices dbs = new DBservices();
        //    return dbs.DeleteUserByEmail(email);
        //}
    }
}
