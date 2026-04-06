using BookASpace.Models;

namespace BookASpace.Views;

public partial class BookingResultPage : ContentPage
{
    public BookingResultPage(BookingResultKind kind, string detailMessage)
    {
        InitializeComponent();
        Title = kind == BookingResultKind.Updated ? "Updated" : "Cancelled";
        TitleLabel.Text = kind == BookingResultKind.Updated
            ? "Booking updated"
            : "Booking cancelled";
        BodyLabel.Text = detailMessage;
    }

    private async void OnDoneClicked(object? sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
