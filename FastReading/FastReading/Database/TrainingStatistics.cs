using SQLite;

namespace FastReading
{
    public class TrainingStatistics
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }     // Уникальный идентификатор записи статистики

        public int UserId { get; set; }     // Идентификатор пользователя, к которому относится статистика

        public int TrainingTypeId { get; set; }     // Идентификатор типа тренировки (например, "Шульте", "Быстрое чтение", "Фокус")

        public int DifficultyLevelId { get; set; }  // Идентификатор уровня сложности (например, "Легкий", "Средний", "Сложный")

        public DateTime Date { get; set; }  // Дата тренировки

        public double TimeSpent { get; set; }   // Время, затраченное на тренировку в секундах

        public int Errors { get; set; }     // Количество ошибок, совершенных пользователем во время тренировки
    }
}
