using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Maui.Skia; // Для использования Skia
using FastReading.Database;
using Microsoft.Maui.Controls;
using System;
using System.Linq; // Для использования LINQ

namespace FastReading
{
    public partial class StatisticsPage : ContentPage
    {
        private readonly DatabaseHelper _databaseHelper;

        public StatisticsPage()
        {
            InitializeComponent();
            _databaseHelper = ((App)Application.Current).Database;  // Получаем экземпляр DatabaseHelper из приложения
            LoadStatistics();  // Загружаем статистику при инициализации страницы
        }

        private async void LoadStatistics()
        {
            var userId = Preferences.Get("UserId", -1);
            if (userId == -1)
            {
                await DisplayAlert("Ошибка", "Пожалуйста, авторизуйтесь", "OK");
                return;
            }

            // Получаем статистику для авторизованного пользователя
            var statistics = await _databaseHelper.GetTrainingStatisticsByUserAsync(userId);

            // Если нет статистики, выводим сообщение
            if (statistics == null || statistics.Count == 0)
            {
                await DisplayAlert("Ошибка", "Статистика отсутствует", "OK");
                return;
            }

            // Находим первую дату (базовую)
            var firstDate = statistics.Min(stat => stat.Date);

            // Создаем график
            var plotModel = new PlotModel { Title = "Статистика по ошибкам" };
            var lineSeries = new LineSeries { Title = "Ошибки", Color = OxyColors.Blue };

            // Добавляем точки на график, используя разницу во времени от первой даты
            foreach (var stat in statistics)
            {
                var timeDifference = stat.Date - firstDate; // Разница во времени

                // Преобразуем разницу во времени в дату для оси X
                var timeForX = firstDate.Add(timeDifference);

                // Добавляем точку на график с разницей во времени (по оси X)
                lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timeForX), stat.Errors));
            }

            plotModel.Series.Add(lineSeries);

            // Настройка оси X (Дата)
            plotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Дата и время",
                StringFormat = "dd/MM/yyyy HH:mm",  // Формат: день/месяц/год часы:минуты
                MajorStep = 1,  // Шаг по оси X
                MinorStep = 0.5,  // Мелкие шаги
                MinorGridlineStyle = LineStyle.Solid,
                MajorGridlineStyle = LineStyle.Solid
            });

            // Настройка оси Y (Количество ошибок)
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Количество ошибок"
            });

            // Отображаем график
            PlotView.Model = plotModel; // Устанавливаем модель графика на PlotView
        }
    }
}
