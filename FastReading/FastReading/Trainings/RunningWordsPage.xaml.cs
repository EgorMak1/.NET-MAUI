using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastReading.Database; // ���������� �������� ������ �� ������������ ���� ��� ������ � ����� ������

namespace FastReading.Trainings
{
    public partial class RunningWordsPage : ContentPage
    {
        private List<string> _words; // ������ ���� ��� ����������
        private int _currentWordIndex = 0;
        private DateTime _startTime;
        private double _timeSpent = 0;
        private int _errors = 0;
        private int _correctAnswers = 0;
        private int _speed = 1000; // ��������� �������� (�� ����� �������)

        public RunningWordsPage()
        {
            InitializeComponent();
            _words = new List<string>
            {
                "�����1", "�����2", "�����3", "�����4", "�����5" // ������ ����
            };
            StartExercise();
        }

        // ����� ��� ������� ����������
        private async void StartExercise()
        {
            _startTime = DateTime.Now;
            _currentWordIndex = 0;

            // �������������� ������� ������ ����
            await ShowNextWord();
        }

        // ����� ��� ������ ���������� �����
        private async Task ShowNextWord()
        {
            if (_currentWordIndex >= _words.Count)
            {
                _timeSpent = (DateTime.Now - _startTime).TotalSeconds;
                // ���������� ����������, ���������� ���������
                await DisplayAlert("���������", $"���������� ���������. �����: {_timeSpent} ������, ������: {_errors}, ���������� �������: {_correctAnswers}", "OK");

                // ���������� ����������
                SaveTrainingStatistics();

                return;
            }

            string word = _words[_currentWordIndex];
            WordLabel.Text = word; // ���������� ����� �� ������
            _currentWordIndex++;

            // ��������� ������ ��� ������ ���������� �����
            await Task.Delay(_speed);

            // ��������� � ���������� �����
            await ShowNextWord();
        }

        // ����� ��� �������� ������ ������������
        private void OnAnswerEntered(object sender, EventArgs e)
        {
            var userAnswer = AnswerEntry.Text;

            if (string.IsNullOrEmpty(userAnswer))
            {
                return;
            }

            if (userAnswer.Equals(_words[_currentWordIndex - 1], StringComparison.OrdinalIgnoreCase))
            {
                _correctAnswers++;
                // ���� ���������� �����, ����������� ��������
                _speed = Math.Max(500, _speed - 100); // ����������� �������� 500 ��
            }
            else
            {
                _errors++;
                // ���� ������, ��������� ��������
                _speed = Math.Min(2000, _speed + 100); // ������������ �������� 2000 ��
            }

            // ������� ���� �����
            AnswerEntry.Text = string.Empty;
        }

        // ����� ��� ���������� ���������� � ���� ������
        private async void SaveTrainingStatistics()
        {
            var userId = Preferences.Get("UserId", -1);
            if (userId == -1) return;

            // ������� ������ ���������� ����������
            var trainingStatistics = new TrainingStatistics
            {
                UserId = userId,
                TrainingTypeId = 2, // ID ��� ������� �����
                DifficultyLevelId = 1, // ������� ���������
                Date = DateTime.Now,
                TimeSpent = _timeSpent, // ����� ����������
                Errors = _errors // ���������� ������
            };

            // �������� ��������� DatabaseHelper
            var databaseHelper = ((App)Application.Current).Database;

            // ��������� ����������
            await databaseHelper.AddTrainingStatisticsAsync(trainingStatistics);
        }
    }
}
