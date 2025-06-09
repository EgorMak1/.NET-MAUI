using SQLite;

namespace FastReading.Database
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Username { get; set; }

        [NotNull]
        public string Password { get; set; }
    }
}
