using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FastReading.Database; // Необходимо добавить ссылку на пространство имен для работы с базой данных

namespace FastReading.Trainings
{
    public partial class RunningWordsPage : ContentPage
    {
        private List<string> _words; // Список слов для тренировки
        private int _currentWordIndex = 0;
        private DateTime _startTime;
        private double _timeSpent = 0;
        private int _errors = 0;
        private int _correctAnswers = 0;
        private int _speed = 1000; // Начальная скорость (мс между словами)

        public RunningWordsPage()
        {
            InitializeComponent();
            _words = new List<string>
            {
                "слово1", "слово2", "слово3", "слово4", "слово5" // Пример слов
            };
            StartExercise();
        }

        // Метод для запуска тренировки
        private async void StartExercise()
        {
            _startTime = DateTime.Now;
            _currentWordIndex = 0;

            // Инициализируем процесс показа слов
            await ShowNextWord();
        }

        // Метод для показа следующего слова
        private async Task ShowNextWord()
        {
            if (_currentWordIndex >= _words.Count)
            {
                _timeSpent = (DateTime.Now - _startTime).TotalSeconds;
                // Завершение тренировки, отображаем результат
                await DisplayAlert("Завершено", $"Тренировка завершена. Время: {_timeSpent} секунд, Ошибки: {_errors}, Правильных ответов: {_correctAnswers}", "OK");

                // Сохранение статистики
                SaveTrainingStatistics();

                return;
            }

            string word = _words[_currentWordIndex];
            WordLabel.Text = word; // Отображаем слово на экране
            _currentWordIndex++;

            // Запускаем таймер для показа следующего слова
            await Task.Delay(_speed);

            // Переходим к следующему слову
            await ShowNextWord();
        }

        // Метод для проверки ответа пользователя
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
                // Если правильный ответ, увеличиваем скорость
                _speed = Math.Max(500, _speed - 100); // Минимальная скорость 500 мс
            }
            else
            {
                _errors++;
                // Если ошибся, уменьшаем скорость
                _speed = Math.Min(2000, _speed + 100); // Максимальная скорость 2000 мс
            }

            // Очищаем поле ввода
            AnswerEntry.Text = string.Empty;
        }

        // Метод для сохранения статистики в базе данных
        private async void SaveTrainingStatistics()
        {
            var userId = Preferences.Get("UserId", -1);
            if (userId == -1) return;

            // Создаем объект статистики тренировки
            var trainingStatistics = new TrainingStatistics
            {
                UserId = userId,
                TrainingTypeId = 2, // ID для Бегущие слова
                DifficultyLevelId = 1, // Уровень сложности
                Date = DateTime.Now,
                TimeSpent = _timeSpent, // Время тренировки
                Errors = _errors // Количество ошибок
            };

            // Получаем экземпляр DatabaseHelper
            var databaseHelper = ((App)Application.Current).Database;

            // Сохраняем статистику
            await databaseHelper.AddTrainingStatisticsAsync(trainingStatistics);
        }
    }
}
