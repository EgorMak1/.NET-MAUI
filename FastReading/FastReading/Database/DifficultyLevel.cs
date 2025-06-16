using SQLite;

public class DifficultyLevel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Level { get; set; } // Например: "Легкий", "Средний", "Сложный"
}
