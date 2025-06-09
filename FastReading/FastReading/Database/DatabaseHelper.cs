using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FastReading.Database
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            // Создаем или открываем базу данных SQLite
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait(); // Создание таблицы User (если еще нет)
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

        // Метод для получения пользователя по имени
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _database.Table<User>()
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync();
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
