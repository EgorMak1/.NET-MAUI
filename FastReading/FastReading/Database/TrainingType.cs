using SQLite;

namespace FastReading.Database
{
    public class TrainingType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Name { get; set; } // Например: "Шульте", "Быстрое чтение", "Фокус"
    }


}