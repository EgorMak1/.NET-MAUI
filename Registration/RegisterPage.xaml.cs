using Microsoft.Maui.Controls;

namespace App
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();

            // ������� �� ����� ����� ��� ������� �� �����
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnLoginTapped;
            TapGestureRecognizer = tapGestureRecognizer;
        }

        // ����� ��� ������ �����������
        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // �������� ����������
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("������", "����������, ��������� ��� ����", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("������", "������ �� ���������", "OK");
                return;
            }

            // ����� ������ ���� ������ ��� ����������� ������������

            // ������� �� ����� ����� ����� �������� �����������
            await DisplayAlert("�����", "����������� ������ �������", "OK");
            await Navigation.PopAsync();
        }

        // ������� �� ����� �����
        private async void OnLoginTapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
