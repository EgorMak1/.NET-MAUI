// Когда вы будете готовы добавить хранение статистики, вам нужно будет расширить базу данных, добавив таблицу для статистики, например:

using SQLite;



public class ExerciseStats
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [NotNull]
    public string Username { get; set; }

    [NotNull]
    public double TimeSpent { get; set; } // Время выполнения упражнения

    [NotNull]
    public DateTime Date { get; set; } // Дата тренировки
}
