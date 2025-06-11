using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Maui.Skia; // ��� ������������� Skia
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
            LoadStatistics();  // ��������� ���������� ��� ������������� ��������
        }

        private async void LoadStatistics()
        {
            var userId = Preferences.Get("UserId", -1);
            if (userId == -1)
            {
                await DisplayAlert("������", "����������, �������������", "OK");
                return;
            }

            //var statistics = await _databaseHelper.GetTrainingStatisticsByUserIdAsync(userId);

            // ������� ������
            var plotModel = new PlotModel { Title = "���������� �� �������" };
            var lineSeries = new LineSeries { Title = "������", Color = OxyColors.Blue };

            // ��������� ����� �� ������
            //foreach (var stat in statistics)
            //{
            //    var dateTime = DateTime.Parse(stat.Date.ToString());
            //    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(dateTime), stat.Errors));
            //}

            plotModel.Series.Add(lineSeries);

            // ��������� �������
            plotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "hh:mm:ss",
                Title = "�����"
            });
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "���������� ������"
            });

            // ���������� ������
            //PlotView.Model = plotModel; // ������������� ������ ������� �� PlotView
        }
    }
}
