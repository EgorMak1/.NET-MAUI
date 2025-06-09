using Microsoft.Maui.Controls;

namespace App
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();

            // Переход на экран входа при нажатии на текст
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnLoginTapped;
            TapGestureRecognizer = tapGestureRecognizer;
        }

        // Метод для кнопки регистрации
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Проверка валидности
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
                return;
            }

            // Здесь должна быть логика для регистрации пользователя

            // Переход на экран входа после успешной регистрации
            await DisplayAlert("Успех", "Регистрация прошла успешно", "OK");
            await Navigation.PopAsync();
        }

        // Переход на экран входа
        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
