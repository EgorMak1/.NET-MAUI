namespace FastReading
{
    public partial class StatisticsSelectionPage : ContentPage
    {
        public StatisticsSelectionPage()
        {
            InitializeComponent();
        }

        private async void OnShulteStatisticsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ShulteTableStatisticsPage());
        }

        private async void OnRunningWordsStatisticsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RunningWordsStatisticsPage());
        }
    }
}
