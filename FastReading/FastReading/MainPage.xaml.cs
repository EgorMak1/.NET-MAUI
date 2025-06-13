using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace FastReading
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            DisplayUserInfo();
            UpdateAuthButton();
        }

        // Обработчик для кнопки "Начать тренировку"
        private async void OnStartTrainingClicked(object sender, EventArgs e)
        {
            // Логика для начала тренировки
            await Navigation.PushAsync(new ExerciseSelectionPage());
        }

        // Обработчик для кнопки "Статистика"
        private async void OnViewStatisticsClicked(object sender, EventArgs e)
        {
            var userId = Preferences.Get("UserId", -1);

            // Если UserId равен -1, значит пользователь не авторизован
            if (userId == -1)
            {
                // Показываем предупреждающее сообщение
                await DisplayAlert("Статистика", "Пожалуйста, авторизуйтесь для доступа к статистике.", "OK");
            }
            else
            {
                // Если пользователь авторизован, переходим на страницу статистики
                await Navigation.PushAsync(new StatisticsPage());
            }

        }

        private void DisplayUserInfo()
        {
            // Получаем UserId и Username из Preferences
            var userId = Preferences.Get("UserId", -1);
            var username = Preferences.Get("Username", "Не авторизован");

            // Отображаем UserId и Username в метках
            UserIdLabel.Text = userId.ToString();
            UsernameLabel.Text = username;
        }

        // Обработчик для кнопки "Авторизация"
        private void UpdateAuthButton()
        {
            var userId = Preferences.Get("UserId", -1);  // Если нет данных, по умолчанию будет -1
            if (userId == -1)
            {
                // Если пользователь не авторизован, показываем кнопку "Войти"

                AuthButton.Text = "Войти";

            }
            else
            {
                // Если пользователь авторизован, показываем кнопку "Выйти"
                AuthButton.Text = "Выйти";
            }
        }
        private async void OnAuthButtonClicked(object sender, EventArgs e)
        {
            var userId = Preferences.Get("UserId", -1);  // Получаем UserId из Preferences

            if (userId == -1)
            {
                // Если пользователь не авторизован, переходим на страницу авторизации
                await Navigation.PushAsync(new LoginPage());
            }
            else
            {
                // Если пользователь авторизован, выходим из системы
                Preferences.Remove("UserId");  // Удаляем UserId
                Preferences.Remove("Username");  // Удаляем имя пользователя

                // Обновляем интерфейс
                DisplayUserInfo();
                UpdateAuthButton();  // Обновляем кнопку после выхода из системы


            }
        }

        // Обработчик для кнопки "Выйти"
        private void OnExitClicked(object sender, EventArgs e)
        {
            // Логика для выхода из приложения
            System.Environment.Exit(0);
        }
    }
}