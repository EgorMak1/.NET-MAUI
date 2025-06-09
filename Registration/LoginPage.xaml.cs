using Microsoft.Maui.Controls;

namespace App
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            // ������� �� ����� ����������� ��� ������� �� �����
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnRegisterTapped;
            TapGestureRecognizer = tapGestureRecognizer;
        }

        // ����� ��� ������ �����
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // �������� ���������� (������)
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("������", "����������, ��������� ��� ����", "OK");
                return;
            }

            // ����� ������ ���� ������ ��� �������������� ������������

            // ������� � �������� ����� ����� ��������� �����
            await Navigation.PushAsync(new MainPage());
        }

        // ������� �� ����� �����������
        private async void OnRegisterTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
