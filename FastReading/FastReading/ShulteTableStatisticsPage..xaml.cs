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
    public partial class ShulteTableStatisticsPage : ContentPage
    {
        private readonly DatabaseHelper _databaseHelper;

        public ShulteTableStatisticsPage()
        {
            InitializeComponent();
            _databaseHelper = ((App)Application.Current).Database; // Получаем экземпляр базы данных
            LoadShulteStatistics();  // Загружаем статистику для Таблицы Шульте
        }

        private async void LoadShulteStatistics()
        {
            var userId = Preferences.Get("UserId", -1);
            if (userId == -1)
            {
                await DisplayAlert("Ошибка", "Пожалуйста, авторизуйтесь", "OK");
                return;
            }

            // Получаем статистику только для тренировки "Таблица Шульте"
            var statistics = await _databaseHelper.GetShulteStatisticsAsync(userId);

            // Если нет статистики, выводим сообщение
            if (statistics == null || statistics.Count == 0)
            {
                await DisplayAlert("Ошибка", "Статистика для Таблицы Шульте отсутствует", "OK");
                return;
            }

            // Здесь будет логика для отображения статистики (графика, таблицы и т. д.)
            var plotModel = new PlotModel { Title = "Статистика по ошибкам Таблицы Шульте" };

            // Линия для графика
            var lineSeries = new LineSeries { Title = "Ошибки", Color = OxyColors.Blue };

            // Добавляем точки на график
            for (int i = 0; i < statistics.Count; i++)
            {
                var stat = statistics[i];
                lineSeries.Points.Add(new DataPoint(i, stat.Errors));  // Индекс i вместо времени
            }

            plotModel.Series.Add(lineSeries);

            // Настройка оси X (категории с датами)
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Дата",
                Angle = 45  // Наклон подписей для улучшения читаемости
            };

            // Добавляем даты как метки оси X
            foreach (var stat in statistics)
            {
                categoryAxis.Labels.Add(stat.Date.ToString("dd.MM HH:mm")); // Можно менять формат
            }

            plotModel.Axes.Add(categoryAxis);

            // Настройка оси Y (Количество ошибок)
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Количество ошибок"
            });

            // Отображаем график
            PlotView.Model = plotModel;
        }

    }

}
