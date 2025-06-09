using Microsoft.Maui.Controls;

namespace FastReading
{
    public partial class ExerciseSelectionPage : ContentPage
    {
        public ExerciseSelectionPage()
        {
            InitializeComponent();
        }

        // Обработчик для кнопки "Таблица Шульте"
        private async void OnShulteTableClicked(object sender, EventArgs e)
        {
            // Переход на страницу тренажера Таблица Шульте
            await Navigation.PushAsync(new ShulteTablePage()); // Updated to use the correct namespace
        }
    }
}
