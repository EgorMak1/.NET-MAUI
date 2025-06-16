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
        private List<string> _words; // Список всех слов из базы данных
        private int _currentWordIndex = 0;
        private double _speed = 500; // Начальная скорость смены слов (0.5 секунды)
        private int _errors = 0;
        private int _correctAnswers = 0;
        private List<string> _shownWords = new List<string>(); // Слова, которые были показаны пользователю
        private List<Label> _wordLabels = new List<Label>(); // Список для хранения ссылок на метки (слова)

        public RunningWordsPage()
        {
            InitializeComponent();
            LoadWordsFromDatabase(); // Загружаем слова из базы данных
        }

        // Загружаем слова из базы данных и перемешиваем их случайным образом
        private async void LoadWordsFromDatabase()
        {
            var databaseHelper = ((App)Application.Current).Database;
            var wordsFromDb = await databaseHelper.GetWordsAsync(); // Получаем все слова из базы данных
            _words = wordsFromDb.Select(w => w.WordText).ToList();

            // Перемешиваем слова случайным образом
            Random rng = new Random();
            _words = _words.OrderBy(x => rng.Next()).ToList();

            StartExercise();
        }

        // Метод для запуска тренировки
        private async void StartExercise()
        {
            // При каждом новом упражнении перемешиваем список слов заново
            Random rng = new Random();
            _shownWords = _words.OrderBy(x => rng.Next()).Take(9).ToList(); // Перемешиваем список и выбираем только 9 слов

            _currentWordIndex = 0;
            _wordLabels.Clear(); // Очищаем список меток
            await ShowNextWord();
        }

        // Метод для показа следующего слова
        private async Task ShowNextWord()
        {
            if (_currentWordIndex >= 9) // Показать только 9 слов
            {
                // Когда все 9 слов показаны, показываем кнопки для выбора слов
                ShowWordSelection();

                // Скрываем все слова после того, как они были показаны
                ClearGrid();
                return;
            }

            string word = _shownWords[_currentWordIndex];
            _shownWords.Add(word); // Добавляем слово в список показанных

            // Отображаем слово в сетке
            DisplayWordsInGrid();

            // Пауза для смены слова
            await Task.Delay((int)_speed);

            // Переходим к следующему слову
            _currentWordIndex++;
            await ShowNextWord();
        }

        // Метод для отображения слов в сетке 3x3
        private void DisplayWordsInGrid()
        {
            // Удаляем предыдущее слово, если оно есть
            if (_wordLabels.Count > 0)
            {
                var lastLabel = _wordLabels.Last();
                lastLabel.Text = ""; // Затираем старое слово (оставляем пустое место)
            }

            // Рассчитываем строку и колонку для текущего слова
            var row = _currentWordIndex / 3; // Рассчитываем строку для нового слова
            var column = _currentWordIndex % 3; // Рассчитываем колонку для нового слова

            // Создаем новое слово
            var label = new Label
            {
                Text = _shownWords.Last(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontSize = 24
            };

            // Размещаем слово в правильной ячейке
            Grid.SetRow(label, row);
            Grid.SetColumn(label, column);
            GridLayout.Children.Add(label);
            _wordLabels.Add(label); // Сохраняем ссылку на метку для скрытия позже
        }

        // Метод для скрытия всех слов в сетке
        private void ClearGrid()
        {
            foreach (var label in _wordLabels)
            {
                GridLayout.Children.Remove(label); // Удаляем все слова из сетки
            }
            _wordLabels.Clear(); // Очищаем список меток
        }

        // Метод для отображения кнопок для выбора слов
        private void ShowWordSelection()
        {
            SelectionGrid.Children.Clear();  // Очистка старых кнопок
            SelectionGrid.IsVisible = true;  // Показываем сетку с кнопками

            // Перемешиваем показанные слова
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

                // Рассчитываем строку и колонку для размещения кнопок в сетке
                int row = i / 3;
                int column = i % 3;
                Grid.SetRow(button, row);
                Grid.SetColumn(button, column);
                SelectionGrid.Children.Add(button);  // Добавляем кнопки в сетку
            }
        }

        // Обработчик для выбора слова пользователем
        private async void WordSelected(object sender, EventArgs e)
        {
            var selectedWord = (sender as Button)?.Text; // Получаем выбранное слово

            // Проверка, выбрал ли пользователь правильное слово
            bool isCorrect = _shownWords.Last() == selectedWord;

            if (isCorrect)
            {
                _correctAnswers++;
                // Уменьшаем скорость при правильном ответе (уменьшаем интервал)
                _speed = Math.Max(1, _speed - 100); // Уменьшаем на 0.1 секунды, но не ниже 1 миллисекунды
                await DisplayAlert("Правильный выбор", $"Вы выбрали правильное слово! Текущая скорость: {_speed / 1000} сек", "OK");
            }
            else
            {
                _errors++;
                // Увеличиваем скорость при ошибке (увеличиваем интервал)
                _speed += 100; // Увеличиваем на 0.1 секунды
                await DisplayAlert("Неправильный выбор", $"Вы выбрали неправильное слово. Попробуйте еще раз. Текущая скорость: {_speed / 1000} сек", "OK");
            }

            // Сохраняем статистику в базу данных
            await SaveTrainingStatistics(isCorrect);

            // Очищаем предыдущие кнопки и начинаем следующее упражнение
            SelectionGrid.Children.Clear();
            StartExercise(); // Начинаем упражнение заново с новым списком слов
        }

        // Метод для сохранения статистики в базе данных
        private async Task SaveTrainingStatistics(bool isCorrect)
        {
            var userId = Preferences.Get("UserId", -1);
            if (userId == -1) return;

            var trainingStatistics = new TrainingStatistics
            {
                UserId = userId,
                TrainingTypeId = 2, // ID для Бегущие слова
                DifficultyLevelId = 1, // Уровень сложности
                Date = DateTime.Now,
                Errors = _errors,
                TimeSpent = 0, // Убираем итоговое время выполнения упражнения
            };

            // Получаем экземпляр DatabaseHelper
            var databaseHelper = ((App)Application.Current).Database;

            // Сохраняем статистику
            await databaseHelper.AddTrainingStatisticsAsync(trainingStatistics);
        }
    }
}
