using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FastReading.Database;

namespace FastReading
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            // Создаем или открываем базу данных SQLite
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait(); // Создание таблицы User (если еще нет)
            _database.CreateTableAsync<TrainingStatistics>().Wait(); // Add this line
        }

        // Сохранение статистики тренировки
        public async Task<int> AddTrainingStatisticsAsync(TrainingStatistics stats)
        {
            return await _database.InsertAsync(stats); // Вставка данных
        }

        // Получение статистики для конкретного пользователя
        public async Task<List<TrainingStatistics>> GetTrainingStatisticsByUserAsync(int userId)
        {
            return await _database.Table<TrainingStatistics>()
                .Where(x => x.UserId == userId)
                .ToListAsync(); // Получаем все записи статистики для пользователя
        }

        // Получение пользователя по имени
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _database.Table<User>()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync(); // Получение пользователя по имени
        }


        // Метод для добавления пользователя
        public async Task<int> AddUserAsync(User user)
        {
            return await _database.InsertAsync(user);
        }

        // Метод для получения всех пользователей
        public async Task<List<User>> GetUsersAsync()
        {
            return await _database.Table<User>().ToListAsync();
        }



        // Метод для создания таблицы статистики
        public async Task<int> AddExerciseStatAsync(ExerciseStats stat)
        {
            return await _database.InsertAsync(stat);
        }


        // Получение статистики
        public async Task<List<ExerciseStats>> GetExerciseStatsAsync(string username)
        {
            return await _database.Table<ExerciseStats>()
                                   .Where(x => x.Username == username)
                                   .ToListAsync();
        }

    }
}
