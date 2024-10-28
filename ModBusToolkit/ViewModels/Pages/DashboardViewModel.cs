namespace ModBusToolkit.ViewModels.Pages
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _counter = 0;

        //[ObservableProperty]
        //private List<>

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter++;
        }
    }
}
