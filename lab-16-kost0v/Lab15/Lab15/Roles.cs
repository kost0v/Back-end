namespace Lab15
{
    public static class PredefinedUsers
    {
        public static readonly List<User> Users = new List<User>
 {
 new User { Username = "admin", Password = "admin", Role = "Admin" },
 new User { Username = "user", Password = "user", Role = "User" }
 };
    }
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
