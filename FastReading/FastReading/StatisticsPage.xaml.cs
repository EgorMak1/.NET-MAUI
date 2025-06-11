using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Maui.Skia; // Для использования Skia
using FastReading.Database;
using Microsoft.Maui.Controls;

namespace FastReading
{
    public partial class StatisticsPage : ContentPage
    {
        private readonly DatabaseHelper _databaseHelper;

        public StatisticsPage()
        {
            InitializeComponent();
            _databaseHelper = ((App)Application.Current).Database;
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

            //var statistics = await _databaseHelper.GetTrainingStatisticsByUserIdAsync(userId);

            // Создаем график
            var plotModel = new PlotModel { Title = "Статистика по ошибкам" };
            var lineSeries = new LineSeries { Title = "Ошибки", Color = OxyColors.Blue };

            // Добавляем точки на график
            //foreach (var stat in statistics)
            //{
            //    var dateTime = DateTime.Parse(stat.Date.ToString());
            //    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime), stat.Errors));
            //}

            plotModel.Series.Add(lineSeries);

            // Настройка графика
            plotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "hh:mm:ss",
                Title = "Время"
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Количество ошибок"
            });

            // Отображаем график
            //PlotView.Model = plotModel; // Устанавливаем модель графика на PlotView
        }
    }
}
