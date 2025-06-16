using FastReading.Database;
using Microsoft.Maui.Controls;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace FastReading
{
    public partial class RunningWordsStatisticsPage : ContentPage
    {
        private readonly DatabaseHelper _databaseHelper;

        public RunningWordsStatisticsPage()
        {
            InitializeComponent();
            _databaseHelper = ((App)Application.Current).Database;
            LoadStatistics();
        }

        private async void LoadStatistics()
        {
            var userId = Preferences.Get("UserId", -1);
            if (userId == -1)
            {
                await DisplayAlert("Ошибка", "Пожалуйста, авторизуйтесь", "OK");
                return;
            }

            var statistics = await _databaseHelper.GetTrainingStatisticsByUserAsync(userId);

            // Фильтруем только по Исчезающим словам (TrainingTypeId = 2)
            var runningStats = statistics
                .Where(s => s.TrainingTypeId == 2)
                .ToList();

            if (runningStats.Count == 0)
            {
                await DisplayAlert("Нет данных", "Статистика по Исчезающим словам отсутствует", "OK");
                return;
            }

            var plotModel = new PlotModel { Title = "Ошибки: Исчезающие слова" };
            var lineSeries = new LineSeries { Title = "Ошибки", Color = OxyColors.DarkRed };

            for (int i = 0; i < runningStats.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, runningStats[i].Errors));
            }

            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Дата",
                Angle = 45
            };

            foreach (var stat in runningStats)
            {
                categoryAxis.Labels.Add(stat.Date.ToString("dd.MM HH:mm"));
            }

            plotModel.Series.Add(lineSeries);
            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Количество ошибок"
            });

            PlotView.Model = plotModel;
        }
    }
}
