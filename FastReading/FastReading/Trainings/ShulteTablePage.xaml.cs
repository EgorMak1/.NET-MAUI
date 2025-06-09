using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastReading
{
    public partial class ShulteTablePage : ContentPage
    {
        private int _currentNumber = 1;
        private DateTime _startTime;
        private List<Button> _buttons = new List<Button>();
        private int _errors = 0;

        public ShulteTablePage()
        {
            InitializeComponent();
            InitializeTable();
        }

        // Метод для генерации таблицы
        private void InitializeTable()
        {
            int gridSize = 5; // Размер таблицы (5x5)
            ShulteTableGrid.RowDefinitions.Clear();
            ShulteTableGrid.ColumnDefinitions.Clear();
            _buttons.Clear();

            // Настроим сетку
            for (int i = 0; i < gridSize; i++)
            {
                ShulteTableGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
                ShulteTableGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            // Генерация случайных чисел для таблицы
            var numbers = Enumerable.Range(1, gridSize * gridSize).OrderBy(x => Guid.NewGuid()).ToList();

            // Создаём кнопки для таблицы
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    var button = new Button
                    {
                        Text = numbers[i * gridSize + j].ToString(),
                        FontSize = 20,
                        BackgroundColor = Color.FromArgb("#D3D3D3") // Светло-серый
                    };
                    button.Clicked += OnButtonClicked;

                    _buttons.Add(button);
                    ShulteTableGrid.Children.Add(button);
                    Grid.SetRow(button, i); // Устанавливаем строку
                    Grid.SetColumn(button, j); // Устанавливаем колонку
                }
            }

            // Запуск таймера
            _startTime = DateTime.Now;
            Dispatcher.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                TimerLabel.Text = $"Время: {DateTime.Now - _startTime:hh\\:mm\\:ss}";
                return true; // Продолжаем таймер
            });
        }

        // Обработчик для нажатия кнопки
        private void OnButtonClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            if (button != null)
            {
                int clickedNumber = int.Parse(button.Text);

                if (clickedNumber == _currentNumber)
                {
                    button.BackgroundColor = Colors.Green; // Правильный ответ
                    _currentNumber++;

                    if (_currentNumber > 25) // Все числа найдены
                    {
                        OnFinishTrainingClicked(sender, e);
                    }
                }
                else
                {
                    button.BackgroundColor = Colors.Red; // Неправильный ответ
                    _errors++;
                }
            }
        }

        // Завершение тренировки
        private async void OnFinishTrainingClicked(object sender, EventArgs e)
        {
            // Выводим время и количество ошибок
            var timeSpent = DateTime.Now - _startTime;
            await DisplayAlert("Тренировка завершена", $"Время: {timeSpent:hh\\:mm\\:ss}\nОшибки: {_errors}", "OK");

            // Сохраняем статистику в базе данных
            var dbHelper = ((App)Application.Current).Database;
            var stats = new TrainingStatistics
            {
                Date = DateTime.Now,
                TimeSpent = timeSpent.TotalSeconds,
                Errors = _errors
            };
            await dbHelper.AddTrainingStatisticsAsync(stats);

            // Переход на страницу статистики или главную страницу
            await Navigation.PopAsync();
        }
    }
}
