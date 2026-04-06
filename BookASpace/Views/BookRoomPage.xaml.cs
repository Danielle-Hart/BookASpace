using BookASpace.Models;
using BookASpace.Services;
using BookASpace.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BookASpace.Views;

public partial class BookRoomPage : ContentPage
{
    public BookRoomPage(Room room, Reservation? existing = null)
    {
        InitializeComponent();
        BindingContext = new BookRoomViewModel(
            room,
            existing,
            App.Services.GetRequiredService<BookingService>(),
            App.Services.GetRequiredService<IDialogService>(),
            App.Services.GetRequiredService<IAppNavigator>());

        BookingDatePicker.MinimumDate = DateTime.Today;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        BookingDatePicker.MinimumDate = DateTime.Today;
        if (BookingDatePicker.Date < DateTime.Today && BindingContext is BookRoomViewModel vm)
            vm.BookingDate = DateTime.Today;
    }
}
