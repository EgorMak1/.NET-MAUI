using FastReading.Database;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace FastReading
{
    public partial class RegisterPage : ContentPage
    {
        private readonly Database.DatabaseHelper _databaseHelper;

        
        public RegisterPage()
        {
            InitializeComponent();
            _databaseHelper = ((App)Application.Current).Database;
        }

        // Метод для регистрации пользователя
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Проверка пустых полей
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Ошибка", "Пожалуйста, заполните все поля", "OK");
                return;
            }

            // Проверка на совпадение паролей
            if (password != confirmPassword)
            {
                await DisplayAlert("Ошибка", "Пароли не совпадают", "OK");
                return;
            }

            // Создание нового пользователя
            var newUser = new User
            {
                Username = username,
                Password = password
            };

            // Добавление пользователя в базу данных
            await _databaseHelper.AddUserAsync(newUser);
            await DisplayAlert("Успех", "Регистрация прошла успешно", "OK");

            // Переход на страницу входа
            await Navigation.PopAsync();
        }

        // Переход на страницу входа
        private async void OnLoginTapped(object sender, EventArgs e)
        {
            // Переход на страницу входа
            await Navigation.PushAsync(new LoginPage());
        }

    }
}
