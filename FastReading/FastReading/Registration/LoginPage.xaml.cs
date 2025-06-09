using Microsoft.Maui.Storage;
using System;
using Microsoft.Maui.Controls;

namespace FastReading
{
    public partial class LoginPage : ContentPage
    {
        private readonly DatabaseHelper _databaseHelper;

        public LoginPage()
        {
            InitializeComponent();
            _databaseHelper = ((App)Application.Current).Database; // Получаем экземпляр базы данных
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Проверка пустых полей
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            // Получаем пользователя из базы данных
            var user = await _databaseHelper.GetUserByUsernameAsync(username);

            // Проверка на наличие пользователя
            if (user == null || user.Password != password)
            {
                await DisplayAlert("Ошибка", "Неверный логин или пароль", "OK");
                return;
            }

            // Сохраняем UserId в Preferences
            Preferences.Set("UserId", user.Id);  // Сохраняем UserId в Preferences

            // Переход на страницу выбора тренажёров
            await Navigation.PushAsync(new ExerciseSelectionPage());
        }

        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());

        }
    }
}
