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
            CreateNormalDistributionModel();    // Создаем модель нормального распределения
            this.Loaded += CustomTrackerPage_Loaded;    // Подписываемся на событие загрузки страницы
            _databaseHelper = ((App)Application.Current).Database;  // Получаем экземпляр DatabaseHelper из приложения
            //LoadStatistics();  // Загружаем статистику при инициализации страницы
        }

        //private async void LoadStatistics()
        //{
        //    var userId = Preferences.Get("UserId", -1);
        //    if (userId == -1)
        //    {
        //        await DisplayAlert("Ошибка", "Пожалуйста, авторизуйтесь", "OK");
        //        return;
        //    }

        //    //var statistics = await _databaseHelper.GetTrainingStatisticsByUserIdAsync(userId);

        //    // Создаем график
        //    var plotModel = new PlotModel { Title = "Статистика по ошибкам" };
        //    var lineSeries = new LineSeries { Title = "Ошибки", Color = OxyColors.Blue };

        //    // Добавляем точки на график
        //    //foreach (var stat in statistics)
        //    //{
        //    //    var dateTime = DateTime.Parse(stat.Date.ToString());
        //    //    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime), stat.Errors));
        //    //}

        //    plotModel.Series.Add(lineSeries);

        //    // Настройка графика
        //    plotModel.Axes.Add(new DateTimeAxis
        //    {
        //        Position = AxisPosition.Bottom,
        //        StringFormat = "hh:mm:ss",
        //        Title = "Время"
        //    });
        //    plotModel.Axes.Add(new LinearAxis
        //    {
        //        Position = AxisPosition.Left,
        //        Title = "Количество ошибок"
        //    });

        // Отображаем график
        //PlotView.Model = plotModel; // Устанавливаем модель графика на PlotView

        private void CustomTrackerPage_Loaded(object sender, EventArgs e)   // Обработчик события загрузки страницы
        {
            PlotView.Model = CreateNormalDistributionModel();    // Устанавливаем модель графика на PlotView
        }

        public static PlotModel CreateNormalDistributionModel()     // Метод для создания модели нормального распределения
        {
            // http://en.wikipedia.org/wiki/Normal_distribution

            var plot = new PlotModel
            {
                Title = "Normal distribution",
                Subtitle = "Probability density function"
            };

            plot.Axes.Add(new LinearAxis    // Добавляем ось Y
            {
                Position = AxisPosition.Left,
                Minimum = -0.05,
                Maximum = 1.05,
                MajorStep = 0.2,
                MinorStep = 0.05,
                TickStyle = TickStyle.Inside
            });
            plot.Axes.Add(new LinearAxis    // Добавляем ось X
            {
                Position = AxisPosition.Bottom,
                Minimum = -5.25,
                Maximum = 5.25,
                MajorStep = 1,
                MinorStep = 0.25,
                TickStyle = TickStyle.Inside
            });
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 0.2));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 1));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 5));
            plot.Series.Add(CreateNormalDistributionSeries(-5, 5, -2, 0.5));
            return plot;
        }
        public static DataPointSeries CreateNormalDistributionSeries(double x0, double x1, double mean, double variance, int n = 1001)  // Метод для создания серии нормального распределения
        {
            var ls = new LineSeries // Создаем новую серию линий
            {
                Title = string.Format("μ={0}, σ²={1}", mean, variance)
            };

            for (int i = 0; i < n; i++) // Цикл для добавления точек в серию
            {
                double x = x0 + ((x1 - x0) * i / (n - 1));
                double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
                ls.Points.Add(new DataPoint(x, f));
            }

            return ls; // Возвращаем серию линий с точками нормального распределения
        }
    }
}
