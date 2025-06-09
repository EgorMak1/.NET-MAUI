namespace FastReading
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage()); // Путь к LoginPage
        }
    }
}
