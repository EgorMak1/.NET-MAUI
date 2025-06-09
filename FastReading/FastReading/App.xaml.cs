using Microsoft.Maui.Storage;
using FastReading.Database;

namespace FastReading
{
    public partial class App : Application
    {
        private readonly Database.DatabaseHelper _databaseHelper;

        public App()
        {
            InitializeComponent();

            // Путь к файлу базы данных
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fastreading.db3");
            _databaseHelper = new Database.DatabaseHelper(dbPath);

            MainPage = new NavigationPage(new RegisterPage());
        }

        // Получить экземпляр базы данных
        public Database.DatabaseHelper Database => _databaseHelper;
    }
}
