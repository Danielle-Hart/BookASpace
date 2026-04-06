using BookASpace.Models;

namespace BookASpace.Views;

public partial class ConfirmationPage : ContentPage
{
    public ConfirmationPage(Reservation reservation)
    {
        InitializeComponent();
        SummaryLabel.Text =
            $"{reservation.RoomDisplayName}\n" +
            $"{reservation.Start:MMM d, yyyy · h:mm tt} – {reservation.End:h:mm tt}\n" +
            (string.IsNullOrWhiteSpace(reservation.Notes) ? "" : $"\nNotes: {reservation.Notes}");
    }

    private async void OnDoneClicked(object? sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
