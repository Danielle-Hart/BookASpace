namespace BookASpace.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnAvailableRoomsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//rooms");
    }

    private async void OnMyReservationsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//bookings");
    }

    private async void OnAccountClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//account");
    }
}
