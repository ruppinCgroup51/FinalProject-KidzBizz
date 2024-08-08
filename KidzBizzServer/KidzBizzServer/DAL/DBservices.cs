using System.Data.SqlClient;
using System.Data;
using KidzBizzServer.BL;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {

        // read the connection string from the configuration file
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json").Build();
        string cStr = configuration.GetConnectionString("myProjDB");
        SqlConnection con = new SqlConnection(cStr);
        con.Open();
        return con;
    }

    //-------------------------------------------------------------------------------------------------
    // !!! USER !!!
    //-------------------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------------------
    // This method return all the App Users
    //--------------------------------------------------------------------------------------------------

    public List<User> ReadUsers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<User> users = new List<User>();

        cmd = buildReadStoredProcedureCommand(con, "KBSP_GetUsers");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            User u = new User();
            u.UserId = Convert.ToInt32(dataReader["UserId"]);
            u.Username = dataReader["Username"].ToString();
            u.Password = dataReader["Password"].ToString();
            u.FirstName = dataReader["FirstName"].ToString();
            u.LastName = dataReader["LastName"].ToString();
            u.AvatarPicture = dataReader["AvatarPicture"].ToString();
            u.DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]);
            u.Gender = dataReader["Gender"].ToString();
            u.Score = Convert.ToInt32(dataReader["Score"]);

            users.Add(u);
        }
        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return users;
    }

    private SqlCommand buildReadStoredProcedureCommand(SqlConnection con, string spName)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        return cmd;

    }

    //-------------------------------------------------------------------------------------------------
    // This method registered user 
    //-------------------------------------------------------------------------------------------------

    public int RegisterUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateRegisterUserWithStoredProcedure("KBSP_InsertUser", con, user);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }
    //--------------------------------------------------------------------------------------------------
    // Create the insert user SqlCommand using a stored procedure
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateRegisterUserWithStoredProcedure(String spName, SqlConnection con, User user)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@Username", user.Username);

        cmd.Parameters.AddWithValue("@Password", user.Password);

        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);

        cmd.Parameters.AddWithValue("@LastName", user.LastName);

        cmd.Parameters.AddWithValue("@AvatarPicture", user.AvatarPicture);

        cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);

        cmd.Parameters.AddWithValue("@Gender", user.Gender);

        cmd.Parameters.AddWithValue("@Score", user.Score);


        return cmd;
    }

    //this method delete user

    public int DeleteUser(string username)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateDeleteUserCommandWithStoredProcedure("KBSP_DeleteUser", con, username);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }



    private SqlCommand CreateDeleteUserCommandWithStoredProcedure(String spName, SqlConnection con, string username)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@Username", username);

        return cmd;
    }

    //-------------------------------------------------------------------------------------------------
    // This method update user
    //-------------------------------------------------------------------------------------------------

    public User UpdateUser(User user)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateUserCommandWithStoredProcedure("KBSP_UserUpdate", con, user);             // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected == 1)
            {
                return user;
            }
            return null;
        }


        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // Create the update user SqlCommand using a stored procedure
    //--------------------------------------------------------------------------------------------------
    private SqlCommand CreateUpdateUserCommandWithStoredProcedure(String spName, SqlConnection con, User user)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@Username", user.Username);

        cmd.Parameters.AddWithValue("@Password", user.Password);

        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);

        cmd.Parameters.AddWithValue("@LastName", user.LastName);

        cmd.Parameters.AddWithValue("@AvatarPicture", user.AvatarPicture);

        cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);

        cmd.Parameters.AddWithValue("@Gender", user.Gender);

        cmd.Parameters.AddWithValue("@Score", user.Score);


        return cmd;
    }

    //-------------------------------------------------------------------------------------------------
    // This method login user
    //-------------------------------------------------------------------------------------------------

    public User LoginUser(string username, string password)
    {

        SqlConnection con;
        SqlCommand cmd;
        User authenticatedUser = null;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }


        cmd = buildReadStoredProcedureCommandLoginUser(con, "KBSP_UserLogin", username, password);

        try
        {


            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())
            {
                authenticatedUser = new User
                {
                    UserId = Convert.ToInt32(dataReader["UserId"]),
                    Username = dataReader["Username"].ToString(),
                    Password = dataReader["Password"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    AvatarPicture = dataReader["AvatarPicture"].ToString(),
                    DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]),
                    Gender = dataReader["Gender"].ToString(),
                    Score = Convert.ToInt32(dataReader["Score"])

                };

            }
            return authenticatedUser;
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }


    }

    private SqlCommand buildReadStoredProcedureCommandLoginUser(SqlConnection con, String spName, string username, string password)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@Username", username);
        cmd.Parameters.AddWithValue("@Password", password);

        return cmd;
    }

    //-------------------------------------------------------------------------------------------------
    // This method read user by username
    //-------------------------------------------------------------------------------------------------

    public User ReadUserByUsername(string username)
    {
        SqlConnection con;
        SqlCommand cmd;
        User user = null;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = buildReadStoredProcedureCommandReadUserByUsername(con, "KBSP_GetUserByUsername", username);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dataReader.Read())
            {
                user = new User
                {
                    Username = dataReader["Username"].ToString(),
                    Password = dataReader["Password"].ToString(),
                    FirstName = dataReader["FirstName"].ToString(),
                    LastName = dataReader["LastName"].ToString(),
                    AvatarPicture = dataReader["AvatarPicture"].ToString(),
                    DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]),
                    Gender = dataReader["Gender"].ToString(),
                    Score = Convert.ToInt32(dataReader["Score"])
                };
            }
            return user;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    private SqlCommand buildReadStoredProcedureCommandReadUserByUsername(SqlConnection con, string spName, string username)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@username", username);

        return cmd;
    }

    //-------------------------------------------------------------------------------------------------
    // !!! PLAYER !!!
    //-------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------
    // This method insert player  
    //-------------------------------------------------------------------------------------------------
    public int InsertPlayer(Player player)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // יצירת חיבור למסד נתונים
        }
        catch (Exception ex)
        {
            // כתיבה ללוג
            throw (ex);
        }

        cmd = CreateInsertPlayerCommand("KBSP_InsertPlayer", con, player); // יצירת פקודת SQL

        try
        {
            cmd.ExecuteNonQuery();
            // Get the PlayerId from the result
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    player.PlayerId = reader.GetInt32(0);
                }
            }

            return player.PlayerId;
        }
        catch (Exception ex)
        {
            // כתיבה ללוג
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close(); // סגירת חיבור למסד נתונים
            }
        }
    }

    private SqlCommand CreateInsertPlayerCommand(String spName, SqlConnection con, Player player)
    {
        SqlCommand cmd = new SqlCommand(); // יצירת אובייקט פקודה

        cmd.Connection = con;            // הגדרת החיבור לפקודה
        cmd.CommandText = spName;        // שם ה-Stored Procedure
        cmd.CommandType = CommandType.StoredProcedure; // סוג הפקודה

        // הוספת פרמטרים לפקודה
        cmd.Parameters.AddWithValue("@UserId", player.User.UserId);
        cmd.Parameters.AddWithValue("@CurrentPosition", player.CurrentPosition);
        cmd.Parameters.AddWithValue("@CurrentBalance", player.CurrentBalance);
        cmd.Parameters.AddWithValue("@PlayerStatus", player.PlayerStatus);
        cmd.Parameters.AddWithValue("@LastDiceResult", player.LastDiceResult);
        cmd.Parameters.AddWithValue("@PlayerType", player.PlayerType);

        return cmd;
    }

    //--------------------------------------------------------------------------------------------------
    // This method return all the App Players
    //--------------------------------------------------------------------------------------------------

    public List<Player> ReadPlayers()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Player> players = new List<Player>();


        cmd = buildReadStoredProcedureCommand(con, "KBSP_GetPlayers");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Player player = new Player();
            player.User = new User();
            player.PlayerId = Convert.ToInt32(dataReader["PlayerId"]);
            player.User.Username = dataReader["Username"].ToString();
            player.User.AvatarPicture = dataReader["AvatarPicture"].ToString();
            player.CurrentPosition = Convert.ToInt32(dataReader["CurrentPosition"]);
            player.CurrentBalance = Convert.ToDouble(dataReader["CurrentBalance"]);
            player.PlayerStatus = dataReader["PlayerStatus"].ToString();
            player.PlayerType = Convert.ToInt32(dataReader["PlayerType"]);
            player.LastDiceResult = Convert.ToInt32(dataReader["LastDiceResult"]);
            player.Properties = ReadPropertiesByPlayerId(player.PlayerId);
            players.Add(player);
        }
        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return players;
    }
    //--------------------------------------------------------------------------------------------------
    // This method adds a property to a player
    //--------------------------------------------------------------------------------------------------
    public int AddPropertyToPlayer(int playerId, int propertyId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateAddPropertyToPlayerCommand("KBSP_AddPropertyToPlayer", con, playerId, propertyId); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    private SqlCommand CreateAddPropertyToPlayerCommand(String spName, SqlConnection con, int playerId, int propertyId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@PlayerId", playerId);
        cmd.Parameters.AddWithValue("@PropertyId", propertyId);

        return cmd;
    }

    //-------------------------------------------------------------------------------------------------
    // This method update Player
    //-------------------------------------------------------------------------------------------------

    public Player UpdatePlayer(Player player)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // יצירת חיבור למסד נתונים
        }
        catch (Exception ex)
        {
            // כתיבה ללוג
            throw (ex);
        }

        cmd = CreateUpdatePlayerCommand("KBSP_PlayerUpdate", con, player); // יצירת פקודת SQL

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected == 1)
            {
                return player;
            }
            return null;
        }
        catch (Exception ex)
        {
            // כתיבה ללוג
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close(); // סגירת חיבור למסד נתונים
            }
        }
    }

    private SqlCommand CreateUpdatePlayerCommand(String spName, SqlConnection con, Player player)
    {
        SqlCommand cmd = new SqlCommand(); // יצירת אובייקט פקודה

        cmd.Connection = con;            // הגדרת החיבור לפקודה
        cmd.CommandText = spName;        // שם ה-Stored Procedure
        cmd.CommandType = CommandType.StoredProcedure; // סוג הפקודה

        // הוספת פרמטרים לפקודה
        cmd.Parameters.AddWithValue("@PlayerId", player.PlayerId);
        cmd.Parameters.AddWithValue("@CurrentPosition", player.CurrentPosition);
        cmd.Parameters.AddWithValue("@CurrentBalance", player.CurrentBalance);
        cmd.Parameters.AddWithValue("@PlayerStatus", player.PlayerStatus);
        cmd.Parameters.AddWithValue("@LastDiceResult", player.LastDiceResult);

        return cmd;
    }
    //--------------------------------------------------------------------------------------------------
    // This method return player by id
    //--------------------------------------------------------------------------------------------------
    public Player GetPlayerById(int playerId)
    {
        SqlConnection con;
        SqlCommand cmd;
        Player player = null;

        try
        {
            con = connect("myProjDB"); // Create a connection to the database
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateGetPlayerByIdCommandWithStoredProcedure("KBSP_GetPlayerDetalis", con, playerId);

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dataReader.Read())
            {
                player = new Player
                {

                    PlayerId = Convert.ToInt32(dataReader["PlayerId"]),
                    CurrentBalance = Convert.ToDouble(dataReader["CurrentBalance"]),
                    CurrentPosition = Convert.ToInt32(dataReader["CurrentPosition"]),
                    PlayerStatus = dataReader["PlayerStatus"].ToString(),
                    LastDiceResult = Convert.ToInt32(dataReader["LastDiceResult"]),
                    User = new User
                    {
                        Username = dataReader["Username"].ToString(),
                        AvatarPicture = dataReader["AvatarPicture"].ToString(),
                        UserId = Convert.ToInt32(dataReader["UserId"]),
                        Gender = dataReader["Gender"].ToString(),
                        DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]),
                        FirstName = dataReader["FirstName"].ToString(),
                        LastName = dataReader["LastName"].ToString(),
                        Score = Convert.ToInt32(dataReader["Score"]),
                        Password = dataReader["Password"].ToString()

                    },
                    // Include other properties as needed
                    Properties = ReadPropertiesByPlayerId(Convert.ToInt32(dataReader["PlayerId"]))
                };
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

        return player;
    }


    private SqlCommand CreateGetPlayerByIdCommandWithStoredProcedure(String spName, SqlConnection con, int playerId)
    {
        SqlCommand cmd = new SqlCommand();

        cmd.Connection = con;
        cmd.CommandText = spName;
        cmd.CommandTimeout = 10;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PlayerId", playerId);

        return cmd;
    }

        // Method to get a game by ID
        public Game GetGameById(int gameId)
        {
            SqlConnection con;
            SqlCommand cmd;
            Game game = null;

            try
            {
                con = connect("myProjDB"); // Create a connection to the database
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            cmd = CreateGetGameByIdCommandWithStoredProcedure("KBSP_GetGameDetails", con, gameId);

            try
            {
                SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dataReader.Read())
                {
                    game = new Game
                    {
                        GameId = Convert.ToInt32(dataReader["GameId"]),
                        NumberOfPlayers = Convert.ToInt32(dataReader["NumberOfPlayers"]),
                        GameDuration = dataReader["GameDuration"].ToString(),
                        GameStatus = dataReader["GameStatus"].ToString(),
                        GameTimestamp = Convert.ToDateTime(dataReader["GameTimestamp"]),
                        // Include other properties as needed
                    };
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

            return game;
        }

        private SqlCommand CreateGetGameByIdCommandWithStoredProcedure(String spName, SqlConnection con, int gameId)
        {
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;
            cmd.CommandText = spName;
            cmd.CommandTimeout = 10;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@GameId", gameId);

            return cmd;
        }


        //-------------------------------------------------------------------------------------------------
        // This method updates player statistics
        //-------------------------------------------------------------------------------------------------
        public void UpdatePlayerStatistics(int playerId, PlayerStatistics statistics)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = new SqlCommand("KBSP_UpdatePlayerStatistics", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PlayerId", playerId);
        cmd.Parameters.AddWithValue("@TotalWins", statistics.TotalWins);
        cmd.Parameters.AddWithValue("@TotalLosses", statistics.TotalLosses);
        cmd.Parameters.AddWithValue("@TotalGamesPlayed", statistics.TotalGamesPlayed);
        cmd.Parameters.AddWithValue("@TotalMoney", statistics.TotalMoney);
        cmd.Parameters.AddWithValue("@TotalPropertiesOwned", statistics.TotalPropertiesOwned);

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    //-------------------------------------------------------------------------------------------------
    // !!! PROPERTY !!!
    //-------------------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------------------
    // This method return properties by id
    //--------------------------------------------------------------------------------------------------
    public List<Property> ReadPropertiesByPlayerId(int playerId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Property> properties = new List<Property>();

        cmd = buildReadStoredProcedureCommandReadPropertiesByPlayer(con, "KBSP_GetPropertiesByPlayer", playerId);

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Property p = new Property();
            p.PropertyId = Convert.ToInt32(dataReader["PropertyId"]);
            p.PropertyName = dataReader["PropertyName"].ToString();
            p.PropertyPrice = Convert.ToDouble(dataReader["PropertyPrice"]);

            properties.Add(p);
        }
        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return properties;
    }

    private SqlCommand buildReadStoredProcedureCommandReadPropertiesByPlayer(SqlConnection con, string spName, int playerId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@PlayerId", playerId);

        return cmd;
    }


    //--------------------------------------------------------------------------------------------------
    // This method return all the Properties
    //--------------------------------------------------------------------------------------------------

    public List<Property> ReadProperties()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Property> properties = new List<Property>();


        cmd = buildReadStoredProcedureCommand(con, "KBSP_GetProperties");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Property p = new Property();
            p.PropertyId = Convert.ToInt32(dataReader["PropertyId"]);
            p.PropertyName = dataReader["PropertyName"].ToString();
            p.PropertyPrice = Convert.ToDouble(dataReader["PropertyPrice"]);

            properties.Add(p);
        }
        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return properties;
    }


    ////-------------------------------------------------------------------------------------------------
    //// !!! QUESTION !!!
    ////-------------------------------------------------------------------------------------------------

    ////--------------------------------------------------------------------------------------------------
    //// This method return all the Questions
    ////--------------------------------------------------------------------------------------------------

    //public List<Question> ReadQuestions()
    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try
    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }
    //    catch (Exception ex)
    //    {
    //        // write to log
    //        throw (ex);
    //    }
    //    List<Question> questions = new List<Question>();


    //    cmd = buildReadStoredProcedureCommand(con, "KBSP_GetQuestions");

    //    SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

    //    while (dataReader.Read())
    //    {
    //        Question q = new Question();
    //        q.QuestionId = Convert.ToInt32(dataReader["QuestionId"]);
    //        q.QuestionText = dataReader["QuestionText"].ToString();

    //        questions.Add(q);
    //    }
    //    if (con != null)
    //    {
    //        // close the db connection
    //        con.Close();
    //    }
    //    return questions;
    //}

    //-------------------------------------------------------------------------------------------------
    // !!! FEEDBACK !!!
    //-------------------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------------------
    // This method return all the Feedbacks
    //--------------------------------------------------------------------------------------------------

    public List<Feedback> ReadFeedback()
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        List<Feedback> feedbacks = new List<Feedback>();


        cmd = buildReadStoredProcedureCommand(con, "KBSP_GetFeedback");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Feedback f = new Feedback();
            f.FeedbackId = Convert.ToInt32(dataReader["FeedbackId"]);
            f.UserId = Convert.ToInt32(dataReader["UserId"]);
            f.FeedbackDescription = dataReader["FeedbackDescription"].ToString();
            f.Rating = Convert.ToInt32(dataReader["Rating"]);

            feedbacks.Add(f);
        }
        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return feedbacks;
    }


    //--------------------------------------------------------------------------------------------------
    // This method insert feedback
    //--------------------------------------------------------------------------------------------------

    public int InsertFeedback(Feedback feedback)

    {
        SqlConnection con;
        SqlCommand cmd;

        try

        {
            con = connect("myProjDB"); // create the connection
        }

        catch (Exception ex)

        {
            throw (ex);
        }

        cmd = CreateInsertFeedbackWithStoredProcedure("KBSP_InsertFeedback", con, feedback);             // create the command

        try

        {
            int numEffected = cmd.ExecuteNonQuery(); // execute the command
            return numEffected;
        }

        catch (Exception ex)

        {
            throw (ex);
        }

        finally
        {
            if (con != null)

            {
                con.Close();
            }
        }
    }

    private SqlCommand CreateInsertFeedbackWithStoredProcedure(string spName, SqlConnection con, Feedback feedback)

    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = spName;          // can be Select, Insert, Update, Delete

        cmd.CommandTimeout = 10;           // Time to wait for the execution. The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text
        cmd.Parameters.AddWithValue("@UserId", feedback.UserId);
        cmd.Parameters.AddWithValue("@FeedbackDescription", feedback.FeedbackDescription);
        cmd.Parameters.AddWithValue("@Rating", feedback.Rating);

        return cmd;
    }

    ////-------------------------------------------------------------------------------------------------
    //// !!! ANSWER !!!
    ////-------------------------------------------------------------------------------------------------

    ////--------------------------------------------------------------------------------------------------
    //// This method insert Answer
    ////--------------------------------------------------------------------------------------------------

    //// לא בטוחה שצריך פונקציה כזאת של הכנסת תשובה מהקליינט 



    //public int InsertAnswer(Answer answer)

    //{

    //    SqlConnection con;
    //    SqlCommand cmd;

    //    try

    //    {
    //        con = connect("myProjDB"); // create the connection
    //    }

    //    catch (Exception ex)

    //    {
    //        // write to log
    //        throw (ex);

    //    }

    //    cmd = CreateInsertAnswerWithStoredProcedure("KBSP_InsertAnswer", con, answer);             // create the command

    //    try

    //    {
    //        int numEffected = cmd.ExecuteNonQuery(); // execute the command
    //        return numEffected;
    //    }

    //    catch (Exception ex)

    //    {
    //        // write to log
    //        throw (ex);
    //    }

    //    finally
    //    {
    //        if (con != null)

    //        {
    //            // close the db connection
    //            con.Close();
    //        }
    //    }

    //}

    //private SqlCommand CreateInsertAnswerWithStoredProcedure(String spName, SqlConnection con, Answer answer)

    //{
    //    SqlCommand cmd = new SqlCommand(); // create the command object

    //    cmd.Connection = con;              // assign the connection to the command object
    //    cmd.CommandText = spName;      // can be Select, Insert, Update, Delete
    //    cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

    //    cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

    //    cmd.Parameters.AddWithValue("@QuestionID", answer.QuestionId);
    //    cmd.Parameters.AddWithValue("@AnswerText", answer.AnswerText);
    //    cmd.Parameters.AddWithValue("@IsCorrect", answer.IsCorrect);
    //    return cmd;

    //}

    //-------------------------------------------------------------------------------------------------
    // !!! GAME !!!
    //-------------------------------------------------------------------------------------------------

    // this method insert game
    public int InsertGame(Game game)
    {
        SqlConnection con;
        SqlCommand cmd;
        int gameId = 0; // Variable to store the game ID

        try
        {
            con = connect("myProjDB"); // Create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateInsertGameWithStoredProcedure("KBSP_InsertGameReturnId", con, game); // Create the command

        try
        {
            SqlParameter outputIdParam = new SqlParameter("@GameId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputIdParam);

            cmd.ExecuteNonQuery(); // Execute the command

            gameId = (int)outputIdParam.Value; // Capture the returned game ID
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close(); // Close the db connection
            }
        }

        return gameId; // Return the game ID
    }


    private SqlCommand CreateInsertGameWithStoredProcedure(String spName, SqlConnection con, Game game)
    {
        SqlCommand cmd = new SqlCommand(); // Create the command object

        cmd.Connection = con; // Assign the connection to the command object

        cmd.CommandText = spName; // Can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10; // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // The type of the command, can also be text

        cmd.Parameters.AddWithValue("@NumberOfPlayers", game.NumberOfPlayers);
        cmd.Parameters.AddWithValue("@GameDuration", game.GameDuration); // Assuming seconds is a suitable unit
        cmd.Parameters.AddWithValue("@GameStatus", game.GameStatus);
        cmd.Parameters.AddWithValue("@GameTimestamp", game.GameTimestamp);

        //SqlParameter outputIdParam = new SqlParameter("@GameId", SqlDbType.Int)
        //{
        //    Direction = ParameterDirection.Output
        //};
        //cmd.Parameters.Add(outputIdParam);

        return cmd;
    }

    // this method read game

    public List<Game> ReadGames()
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        List<Game> games = new List<Game>();

        cmd = buildReadStoredProcedureCommand(con, "KBSP_GetGames");

        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        while (dataReader.Read())
        {
            Game game = new Game();
            game.GameId = Convert.ToInt32(dataReader["GameId"]);
            game.NumberOfPlayers = Convert.ToInt32(dataReader["NumberOfPlayers"]);
            game.GameDuration = dataReader["GameDuration"].ToString();
            game.GameStatus = dataReader["GameStatus"].ToString();
            game.GameTimestamp = Convert.ToDateTime(dataReader["GameTimestamp"]);

            games.Add(game);
        }
        if (con != null)
        {
            // close the db connection
            con.Close();
        }
        return games;
    }



    // this method update game
    public Game UpdateGame(Game game)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateGameCommandWithStoredProcedure("KBSP_GameUpdate", con, game); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected == 1)
            {
                return game;
            }
            return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    private SqlCommand CreateUpdateGameCommandWithStoredProcedure(String spName, SqlConnection con, Game game)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con; // assign the connection to the command object

        cmd.CommandText = spName; // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10; // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@GameId", game.GameId);
        cmd.Parameters.AddWithValue("@NumberOfPlayers", game.NumberOfPlayers);
        cmd.Parameters.AddWithValue("@GameDuration", game.GameDuration);
        cmd.Parameters.AddWithValue("@GameStatus", game.GameStatus);
        cmd.Parameters.AddWithValue("@GameTimestamp", game.GameTimestamp);

        return cmd;
        }
    //------------------------------------------------------------------
    public Card GetCardById(int cardId)
    {
        SqlConnection con;
        SqlCommand cmd;
        Card card = null;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("KBSP_GetCardById", con);
        cmd.Parameters.AddWithValue("@CardID", cardId);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dr.Read())
            {
                card = new Card
                {
                    CardId = Convert.ToInt32(dr["CardID"]),
                    Description = dr["Description"].ToString(),
                    Action = (CardAction)Convert.ToInt32(dr["ActionType"])
                };
            }
            return card;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public CommandCard GetCommandCardDetails(int cardId)
    {
        SqlConnection con;
        SqlCommand cmd;
        CommandCard card = null;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("KBSP_GetCommandCardDetails", con);
        cmd.Parameters.AddWithValue("@CardID", cardId);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dr.Read())
            {
                card = new CommandCard
                {
                    CardId = cardId,
                    Description = dr["Description"].ToString(),
                    Action = CardAction.Command,
                    Amount = Convert.ToDouble(dr["Amount"]),
                    MoveTo = dr["MoveTo"].ToString()
                };
            }
            return card;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public SurpriseCard GetSurpriseCardDetails(int cardId)
    {
        SqlConnection con;
        SqlCommand cmd;
        SurpriseCard card = null;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("KBSP_GetSurpriseCardDetails", con);
        cmd.Parameters.AddWithValue("@CardID", cardId);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dr.Read())
            {
                card = new SurpriseCard
                {
                    CardId = cardId,
                    Description = dr["Description"].ToString(),
                    Action = CardAction.Surprise,
                    Amount = Convert.ToDouble(dr["Amount"])
                };
            }
            return card;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public DidYouKnowCard GetDidYouKnowCardDetails(int cardId)
    {
        SqlConnection con;
        SqlCommand cmd;
        DidYouKnowCard card = null;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("KBSP_GetDidYouKnowCardDetails", con);
        cmd.Parameters.AddWithValue("@CardID", cardId);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dr.Read())
            {
                card = new DidYouKnowCard
                {
                    CardId = cardId,
                    Description = dr["Description"].ToString(),
                    Action = CardAction.DidYouKnow,
                    Question = dr["Question"].ToString(),
                    Answer1 = dr["Answer1"].ToString(),
                    Answer2 = dr["Answer2"].ToString(),
                    CorrectAnswer = dr["CorrectAnswer"].ToString()
                };
            }
            return card;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    private SqlCommand CreateCommandWithStoredProcedure(string spName, SqlConnection con)
    {
        SqlCommand cmd = new SqlCommand
        {
            Connection = con,
            CommandText = spName,
            CommandTimeout = 10,
            CommandType = CommandType.StoredProcedure
        };

        return cmd;
    }

    public bool InsertCard(Card card)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateInsertCardCommandWithStoredProcedure("KBSP_InsertCard", con, card);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected == 1;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public bool UpdateCard(Card card)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateUpdateCardCommandWithStoredProcedure("KBSP_UpdateCard", con, card);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected == 1;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public bool DeleteCard(int cardId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateDeleteCardCommandWithStoredProcedure("KBSP_DeleteCard", con, cardId);

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            return numEffected == 1;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    private SqlCommand CreateInsertCardCommandWithStoredProcedure(string spName, SqlConnection con, Card card)
    {
        SqlCommand cmd = new SqlCommand
        {
            Connection = con,
            CommandText = spName,
            CommandTimeout = 10,
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@Description", card.Description);
        cmd.Parameters.AddWithValue("@ActionType", card.Action);

        return cmd;
    }

    private SqlCommand CreateUpdateCardCommandWithStoredProcedure(string spName, SqlConnection con, Card card)
    {
        SqlCommand cmd = new SqlCommand
        {
            Connection = con,
            CommandText = spName,
            CommandTimeout = 10,
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@CardID", card.CardId);
        cmd.Parameters.AddWithValue("@Description", card.Description);
        cmd.Parameters.AddWithValue("@ActionType", card.Action);

        return cmd;
    }

    private SqlCommand CreateDeleteCardCommandWithStoredProcedure(string spName, SqlConnection con, int cardId)
    {
        SqlCommand cmd = new SqlCommand
        {
            Connection = con,
            CommandText = spName,
            CommandTimeout = 10,
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@CardID", cardId);

        return cmd;
    }

    public List<Card> ReadCards()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<Card> cards = new List<Card>();

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("KBSP_GetAllCards", con);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {
                cards.Add(new Card
                {
                    CardId = Convert.ToInt32(dr["CardID"]),
                    Description = dr["Description"].ToString(),
                    Action = (CardAction)Convert.ToInt32(dr["ActionType"])
                });
            }
            return cards;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<CommandCard> ReadCommandCards()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<CommandCard> cards = new List<CommandCard>();

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("KBSP_GetAllCommandCards", con);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {
                cards.Add(new CommandCard
                {
                    CardId = Convert.ToInt32(dr["CardID"]),
                    Description = dr["Description"].ToString(),
                    Action = CardAction.Command,
                    Amount = Convert.ToDouble(dr["Amount"]),
                    MoveTo = dr["MoveTo"].ToString()
                });
            }
            return cards;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<SurpriseCard> ReadSurpriseCards()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<SurpriseCard> cards = new List<SurpriseCard>();

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("KBSP_GetAllSurpriseCards", con);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {
                cards.Add(new SurpriseCard
                {
                    CardId = Convert.ToInt32(dr["CardID"]),
                    Description = dr["Description"].ToString(),
                    Action = CardAction.Surprise,
                    Amount = Convert.ToDouble(dr["Amount"])
                });
            }
            return cards;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<DidYouKnowCard> ReadDidYouKnowCards()
    {
        SqlConnection con;
        SqlCommand cmd;
        List<DidYouKnowCard> cards = new List<DidYouKnowCard>();

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = CreateCommandWithStoredProcedure("KBSP_GetAllDidYouKnowCards", con);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {
                cards.Add(new DidYouKnowCard
                {
                    CardId = Convert.ToInt32(dr["CardID"]),
                    Description = dr["Description"].ToString(),
                    Action = CardAction.DidYouKnow,
                    Question = dr["Question"].ToString(),
                    Answer1 = dr["Answer1"].ToString(),
                    Answer2 = dr["Answer2"].ToString(),
                    CorrectAnswer = dr["CorrectAnswer"].ToString()
                });
            }
            return cards;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


//--------------------------------------------------------------------

public (decimal, int) GetGameSettings()    // from Game setting table 
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        cmd = buildReadStoredProcedureCommand(con, "KBSP_GetGameSettings");
        SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        while (dataReader.Read())
        {
            decimal startingMoney = Convert.ToDecimal(dataReader["StartingMoney"]);
            int currentLocation = Convert.ToInt32(dataReader["CurrentLocation"]);
            return (startingMoney, currentLocation);

        }
        if (con != null)
        {
            con.Close();
        }
        return (0, 0);
    }


    public void UpdatePlayerPosition(int playerId, int newPosition , int lastDiceResult)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdatePosCommandWithStoredProcedure("KBSP_UpdatePlayerPosition", con, playerId, newPosition, lastDiceResult); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected != 1)
            {
                // Handle the case where the update was not successful
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    private SqlCommand CreateUpdatePosCommandWithStoredProcedure(String spName, SqlConnection con, int playerId, int newPosition, int lastDiceResult)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con; // assign the connection to the command object

        cmd.CommandText = spName; // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10; // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@playerId", playerId);
        cmd.Parameters.AddWithValue("@newPosition", newPosition);
        cmd.Parameters.AddWithValue("@lastDiceResult", lastDiceResult);


        return cmd;
    }

    // this method get rent price by property id

    public decimal GetRentPrice(int propertyId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateGetRentPriceCommandWithStoredProcedure("KBSP_GetRentPrice", con, propertyId); // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dataReader.Read())
            {
                return Convert.ToDecimal(dataReader["RentPrice"]);
            }
            return 0;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateGetRentPriceCommandWithStoredProcedure(String spName, SqlConnection con, int propertyId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con; // assign the connection to the command object

        cmd.CommandText = spName; // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10; // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@PropertyId", propertyId);

        return cmd;
    }

    public decimal GetPlayerBalance(int playerId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateGetPlayerBalanceCommandWithStoredProcedure("KBSP_GetPlayerBalance", con, playerId); // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dataReader.Read())
            {
                return Convert.ToDecimal(dataReader["CurrentBalance"]);
            }
            return 0;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }

    }

    private SqlCommand CreateGetPlayerBalanceCommandWithStoredProcedure(String spName, SqlConnection con, int playerId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con; // assign the connection to the command object

        cmd.CommandText = spName; // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10; // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@PlayerId", playerId);

        return cmd;
    }

    public void UpdatePlayerBalance(int playerId, decimal newBalance)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateUpdateBalanceCommandWithStoredProcedure("KBSP_UpdatePlayerBalance", con, playerId, newBalance); // create the command

        try
        {
            int numEffected = cmd.ExecuteNonQuery();
            if (numEffected != 1)
            {
                // Handle the case where the update was not successful
            }
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    private SqlCommand CreateUpdateBalanceCommandWithStoredProcedure(String spName, SqlConnection con, int playerId, decimal newBalance)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con; // assign the connection to the command object

        cmd.CommandText = spName; // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10; // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@PlayerId", playerId);
        cmd.Parameters.AddWithValue("@NewBalance", newBalance);

        return cmd;
    }

    public Property GetPropertyDetails(int propertyId)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("myProjDB"); // create the connection
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }

        cmd = CreateGetPropertyDetailsCommandWithStoredProcedure("KBSP_GetPropertyDetails", con, propertyId); // create the command

        try
        {
            SqlDataReader dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dataReader.Read())
            {
                return new Property(
                    propertyId: Convert.ToInt32(dataReader["PropertyId"]),
                    propertyName: dataReader["PropertyName"].ToString(),
                    propertyPrice: Convert.ToDouble(dataReader["PropertyPrice"])
                );
            }
            return null;
        }
        catch (Exception ex)
        {
            // write to log
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                // close the db connection
                con.Close();
            }
        }
    }

    private SqlCommand CreateGetPropertyDetailsCommandWithStoredProcedure(String spName, SqlConnection con, int propertyId)
    {
        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con; // assign the connection to the command object

        cmd.CommandText = spName; // the name of the stored procedure 

        cmd.CommandTimeout = 10; // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.StoredProcedure; // the type of the command, can also be text

        cmd.Parameters.AddWithValue("@PropertyId", propertyId);

        return cmd;
    }


}













