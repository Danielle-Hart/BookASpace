using System.Collections.ObjectModel;
using BookASpace.Models;
using BookASpace.Services;
using BookASpace.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookASpace.ViewModels;

public partial class MyReservationsViewModel : ObservableObject
{
    private readonly BookingService _bookingService;
    private readonly RoomService _roomService;
    private readonly IAppNavigator _navigator;
    private readonly IDialogService _dialogs;

    public MyReservationsViewModel(
        BookingService bookingService,
        RoomService roomService,
        IAppNavigator navigator,
        IDialogService dialogs)
    {
        _bookingService = bookingService;
        _roomService = roomService;
        _navigator = navigator;
        _dialogs = dialogs;
        Reservations = new ObservableCollection<Reservation>();
    }

    public ObservableCollection<Reservation> Reservations { get; }

    [ObservableProperty]
    private bool isEmpty = true;

    [RelayCommand]
    public async Task LoadAsync()
    {
        var list = await _bookingService.GetReservationsAsync();
        Reservations.Clear();
        foreach (var r in list)
            Reservations.Add(r);

        IsEmpty = Reservations.Count == 0;
    }

    [RelayCommand]
    private async Task EditAsync(Reservation? reservation)
    {
        if (reservation == null)
            return;

        var room = _roomService.GetRoomById(reservation.RoomId) ?? new Room
        {
            Id = reservation.RoomId,
            Building = string.Empty,
            RoomNumber = reservation.RoomDisplayName,
            Description = "Room details may have changed; times and notes can still be updated.",
            Capacity = 0,
            HasWhiteboard = false,
            HasOutlets = true
        };

        await _navigator.PushAsync(new BookRoomPage(room, reservation));
    }

    [RelayCommand]
    private async Task CancelAsync(Reservation? reservation)
    {
        if (reservation == null)
            return;

        var confirm = await _dialogs.ConfirmAsync(
            "Cancel booking?",
            $"Remove your reservation for {reservation.RoomDisplayName}?",
            "Cancel booking",
            "Keep");

        if (!confirm)
            return;

        await _bookingService.CancelReservationAsync(reservation.Id);
        await LoadAsync();

        var detail =
            $"{reservation.RoomDisplayName}\n" +
            $"{reservation.Start:MMM d, yyyy · h:mm tt} – {reservation.End:h:mm tt}";

        await _navigator.PushAsync(new BookingResultPage(BookingResultKind.Cancelled, detail));
    }
}
