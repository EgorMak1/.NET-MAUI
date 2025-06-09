using SQLite;

namespace FastReading
{
    public class TrainingStatistics
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double TimeSpent { get; set; } // Время выполнения упражнения
        public int Errors { get; set; } // Количество ошибок
    }
}
