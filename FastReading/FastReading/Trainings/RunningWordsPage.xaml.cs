using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FastReading.Database;

namespace FastReading.Trainings
{
    public partial class RunningWordsPage : ContentPage
    {
        private List<string> _words; // ������ ���� ���� �� ���� ������
        private int _currentWordIndex = 0;
        private double _speed = 500; // ��������� �������� ����� ���� (0.5 �������)
        private int _errors = 0;
        private int _correctAnswers = 0;
        private List<string> _shownWords = new List<string>(); // �����, ������� ���� �������� ������������
        private List<Label> _wordLabels = new List<Label>(); // ������ ��� �������� ������ �� ����� (�����)

        public RunningWordsPage()
        {
            InitializeComponent();
            LoadWordsFromDatabase(); // ��������� ����� �� ���� ������
        }

        // ��������� ����� �� ���� ������ � ������������ �� ��������� �������
        private async void LoadWordsFromDatabase()
        {
            var databaseHelper = ((App)Application.Current).Database;
            var wordsFromDb = await databaseHelper.GetWordsAsync(); // �������� ��� ����� �� ���� ������
            _words = wordsFromDb.Select(w => w.WordText).ToList();

            // ������������ ����� ��������� �������
            Random rng = new Random();
            _words = _words.OrderBy(x => rng.Next()).ToList();

            StartExercise();
        }

        // ����� ��� ������� ����������
        private async void StartExercise()
        {
            // ��� ������ ����� ���������� ������������ ������ ���� ������
            Random rng = new Random();
            _shownWords = _words.OrderBy(x => rng.Next()).Take(9).ToList(); // ������������ ������ � �������� ������ 9 ����

            _currentWordIndex = 0;
            _wordLabels.Clear(); // ������� ������ �����
            await ShowNextWord();
        }

        // ����� ��� ������ ���������� �����
        private async Task ShowNextWord()
        {
            if (_currentWordIndex >= 9) // �������� ������ 9 ����
            {
                // ����� ��� 9 ���� ��������, ���������� ������ ��� ������ ����
                ShowWordSelection();

                // �������� ��� ����� ����� ����, ��� ��� ���� ��������
                ClearGrid();
                return;
            }

            string word = _shownWords[_currentWordIndex];
            _shownWords.Add(word); // ��������� ����� � ������ ����������

            // ���������� ����� � �����
            DisplayWordsInGrid();

            // ����� ��� ����� �����
            await Task.Delay((int)_speed);

            // ��������� � ���������� �����
            _currentWordIndex++;
            await ShowNextWord();
        }

        // ����� ��� ����������� ���� � ����� 3x3
        private void DisplayWordsInGrid()
        {
            // ������� ���������� �����, ���� ��� ����
            if (_wordLabels.Count > 0)
            {
                var lastLabel = _wordLabels.Last();
                lastLabel.Text = ""; // �������� ������ ����� (��������� ������ �����)
            }

            // ������������ ������ � ������� ��� �������� �����
            var row = _currentWordIndex / 3; // ������������ ������ ��� ������ �����
            var column = _currentWordIndex % 3; // ������������ ������� ��� ������ �����

            // ������� ����� �����
            var label = new Label
            {
                Text = _shownWords.Last(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 24
            };

            // ��������� ����� � ���������� ������
            Grid.SetRow(label, row);
            Grid.SetColumn(label, column);
            GridLayout.Children.Add(label);
            _wordLabels.Add(label); // ��������� ������ �� ����� ��� ������� �����
        }

        // ����� ��� ������� ���� ���� � �����
        private void ClearGrid()
        {
            foreach (var label in _wordLabels)
            {
                GridLayout.Children.Remove(label); // ������� ��� ����� �� �����
            }
            _wordLabels.Clear(); // ������� ������ �����
        }

        // ����� ��� ����������� ������ ��� ������ ����
        private void ShowWordSelection()
        {
            SelectionGrid.Children.Clear();  // ������� ������ ������
            SelectionGrid.IsVisible = true;  // ���������� ����� � ��������

            // ������������ ���������� �����
            var shuffledWords = _shownWords.Take(9).OrderBy(x => Guid.NewGuid()).ToList();

            for (int i = 0; i < shuffledWords.Count; i++)
            {
                var word = shuffledWords[i];
                var button = new Button
                {
                    Text = word,
                    FontSize = 18
                };
                button.Clicked += WordSelected;

                // ������������ ������ � ������� ��� ���������� ������ � �����
                int row = i / 3;
                int column = i % 3;
                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);
                SelectionGrid.Children.Add(button);  // ��������� ������ � �����
            }
        }

        // ���������� ��� ������ ����� �������������
        private async void WordSelected(object sender, EventArgs e)
        {
            var selectedWord = (sender as Button)?.Text; // �������� ��������� �����

            // ��������, ������ �� ������������ ���������� �����
            bool isCorrect = _shownWords.Last() == selectedWord;

            if (isCorrect)
            {
                _correctAnswers++;
                // ��������� �������� ��� ���������� ������ (��������� ��������)
                _speed = Math.Max(1, _speed - 100); // ��������� �� 0.1 �������, �� �� ���� 1 ������������
                await DisplayAlert("���������� �����", $"�� ������� ���������� �����! ������� ��������: {_speed / 1000} ���", "OK");
            }
            else
            {
                _errors++;
                // ����������� �������� ��� ������ (����������� ��������)
                _speed += 100; // ����������� �� 0.1 �������
                await DisplayAlert("������������ �����", $"�� ������� ������������ �����. ���������� ��� ���. ������� ��������: {_speed / 1000} ���", "OK");
            }

            // ��������� ���������� � ���� ������
            await SaveTrainingStatistics(isCorrect);

            // ������� ���������� ������ � �������� ��������� ����������
            SelectionGrid.Children.Clear();
            StartExercise(); // �������� ���������� ������ � ����� ������� ����
        }

        // ����� ��� ���������� ���������� � ���� ������
        private async Task SaveTrainingStatistics(bool isCorrect)
        {
            var userId = Preferences.Get("UserId", -1);
            if (userId == -1) return;

            var trainingStatistics = new TrainingStatistics
            {
                UserId = userId,
                TrainingTypeId = 2, // ID ��� ������� �����
                DifficultyLevelId = 1, // ������� ���������
                Date = DateTime.Now,
                Errors = _errors,
                TimeSpent = 0, // ������� �������� ����� ���������� ����������
            };

            // �������� ��������� DatabaseHelper
            var databaseHelper = ((App)Application.Current).Database;

            // ��������� ����������
            await databaseHelper.AddTrainingStatisticsAsync(trainingStatistics);
        }
    }
}
