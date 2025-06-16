using Microsoft.Maui.Storage;
using FastReading.Database;

namespace FastReading
{
    public partial class App : Application
    {
        private readonly DatabaseHelper _databaseHelper;

        public App()
        {
            InitializeComponent();

            // Path to the database file
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "fastreading.db3");
            _databaseHelper = new DatabaseHelper(dbPath);

            // Инициализация базовых данных
            //Database.InitializeDefaultsAsync().Wait();

            MainPage = new NavigationPage(new MainPage());
        }

        // Get the database instance
        public DatabaseHelper Database => _databaseHelper;
    }
}
