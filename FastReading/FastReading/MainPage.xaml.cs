using Microsoft.Maui.Controls;

namespace FastReading
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        // Обработчик для кнопки "Начать тренировку"
        private async void OnStartTrainingClicked(object sender, EventArgs e)
        {
            // Логика для начала тренировки
            await DisplayAlert("Начать тренировку", "Вы начали тренировку!", "OK");

            // Можно добавить переход на экран тренировки, если он будет:
            // await Navigation.PushAsync(new TrainingPage());
        }

        // Обработчик для кнопки "Статистика"
        private async void OnViewStatisticsClicked(object sender, EventArgs e)
        {
            // Логика для отображения статистики
            await DisplayAlert("Статистика", "Здесь будет ваша статистика.", "OK");

            // Также, если есть экран статистики:
            // await Navigation.PushAsync(new StatisticsPage());
        }

        // Обработчик для кнопки "Выйти"
        private void OnExitClicked(object sender, EventArgs e)
        {
            // Логика для выхода из приложения
            System.Environment.Exit(0);
        }
    }
}
