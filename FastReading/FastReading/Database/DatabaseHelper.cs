using FastReading.Database;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FastReading
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            // Создаем или открываем базу данных SQLite
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();      // Создание таблицы User (если еще нет)
            _database.CreateTableAsync<TrainingStatistics>().Wait();    // Add this line
            _database.CreateTableAsync<TrainingType>().Wait();  // Создание таблицы TrainingType (если еще нет)
            _database.CreateTableAsync<DifficultyLevel>().Wait();   // Создание таблицы DifficultyLevel (если еще нет)
            _database.CreateTableAsync<TrainingStatistics>().Wait();    // Создание таблицы TrainingStatistics (если еще нет)

        }

        // Получение статистики для конкретного пользователя
        public async Task<List<TrainingStatistics>> GetTrainingStatisticsByUserAsync(int userId)    //Метод для получения статистики тренировок пользователя по его идентификатору
        {
            return await _database.Table<TrainingStatistics>()
                .Where(x => x.UserId == userId)
                .ToListAsync(); // Получаем все записи статистики для пользователя
        }

        // Получение пользователя по имени
        public async Task<User> GetUserByUsernameAsync(string username) // Метод для получения пользователя по имени пользователя
        {
            return await _database.Table<User>()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync(); // Получение пользователя по имени
        }


        // Метод для добавления пользователя
        public async Task<int> AddUserAsync(User user)  // Метод для добавления нового пользователя в базу данных
        {
            return await _database.InsertAsync(user);
        }

        // Метод для получения всех пользователей
        public async Task<List<User>> GetUsersAsync()   // Метод для получения всех пользователей из базы данных
        {
            return await _database.Table<User>().ToListAsync();
        }



        // Метод для создания таблицы статистики
        public async Task<int> AddExerciseStatAsync(ExerciseStats stat) // Метод для добавления статистики упражнения
        {
            return await _database.InsertAsync(stat);
        }


        // Получение статистики
        public async Task<List<ExerciseStats>> GetExerciseStatsAsync(string username)   // Метод для получения статистики по имени пользователя
        {
            return await _database.Table<ExerciseStats>()
                                   .Where(x => x.Username == username)
                                   .ToListAsync();
        }

        public async Task<int> AddTrainingStatisticsAsync(TrainingStatistics session)   //Метод для добавления статистики тренировки
        {
            return await _database.InsertAsync(session);
        }

        public async Task<List<TrainingStatistics>> GetSessionsByUserAsync(int userId)  // Метод для получения всех сессий пользователя
        {
            return await _database.Table<TrainingStatistics>()
                                  .Where(s => s.UserId == userId)
                                  .ToListAsync();
        }

        public async Task<List<TrainingStatistics>> GetTrainingStatisticsWithNamesAsync(int userId)// Метод для получения статистики с именами тренировок
        {
            var query = @"
        SELECT ts.*, tt.Name AS TrainingName
        FROM TrainingStatistics ts
        INNER JOIN TrainingType tt ON ts.TrainingTypeId = tt.Id
        WHERE ts.UserId = ?
    ";

            return await _database.QueryAsync<TrainingStatistics>(query, userId);
        }
    }
}
