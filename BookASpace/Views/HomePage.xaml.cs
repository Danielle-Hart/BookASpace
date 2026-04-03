namespace BookASpace.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnAvailableRoomsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(AvailableRoomsPage));
    }

    private async void OnMyReservationsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MyReservationsPage));
    }
}