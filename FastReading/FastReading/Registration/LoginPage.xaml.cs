using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using FastReading.Database;

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

        // Метод для проверки логина
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

            // Переход на главный экран после успешного входа
            await Navigation.PushAsync(new MainPage());
        }

        // Переход на страницу регистрации
        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
        

    }
}
