using Microsoft.Maui.Controls;

namespace App
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            // Переход на экран регистрации при нажатии на текст
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnRegisterTapped;
            TapGestureRecognizer = tapGestureRecognizer;
        }

        // Метод для кнопки входа
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Проверка валидности (пример)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            // Здесь должна быть логика для аутентификации пользователя

            // Переход в основной экран после успешного входа
            await Navigation.PushAsync(new MainPage());
        }

        // Переход на экран регистрации
        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
