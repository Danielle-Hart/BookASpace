using BookASpace.Models;
using BookASpace.Services;
using BookASpace.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookASpace.ViewModels;

public partial class BookRoomViewModel : ObservableObject
{
    private readonly Room _room;
    private readonly Guid? _existingId;
    private readonly BookingService _bookingService;
    private readonly IDialogService _dialogs;
    private readonly IAppNavigator _navigator;

    public BookRoomViewModel(
        Room room,
        Reservation? existing,
        BookingService bookingService,
        IDialogService dialogs,
        IAppNavigator navigator)
    {
        _room = room;
        _bookingService = bookingService;
        _dialogs = dialogs;
        _navigator = navigator;

        if (existing != null)
        {
            _existingId = existing.Id;
            PageTitle = "Edit booking";
            SaveButtonText = "Save changes";
            BookingDate = existing.Start.Date;
            StartTime = existing.Start.TimeOfDay;
            EndTime = existing.End.TimeOfDay;
            Notes = existing.Notes ?? string.Empty;
        }
        else
        {
            PageTitle = "Book room";
            SaveButtonText = "Confirm booking";
            BookingDate = DateTime.Today;
            StartTime = TimeSpan.FromHours(10);
            EndTime = TimeSpan.FromHours(11);
        }

        RoomDisplayName = room.DisplayName;
    }

    public string RoomDisplayName { get; }

    [ObservableProperty]
    private string pageTitle;

    [ObservableProperty]
    private string saveButtonText;

    [ObservableProperty]
    private DateTime bookingDate;

    [ObservableProperty]
    private TimeSpan startTime;

    [ObservableProperty]
    private TimeSpan endTime;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
            return;

        var start = BookingDate.Date + StartTime;
        var end = BookingDate.Date + EndTime;

        if (end <= start)
        {
            await _dialogs.AlertAsync("Invalid time", "End time must be after start time.");
            return;
        }

        if (start < DateTime.Now)
        {
            await _dialogs.AlertAsync("Invalid time", "You cannot save a time that has already passed.");
            return;
        }

        try
        {
            IsBusy = true;
            var trimmedNotes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim();

            if (_existingId != null)
            {
                var updated = new Reservation
                {
                    Id = _existingId.Value,
                    RoomId = _room.Id,
                    RoomDisplayName = _room.DisplayName,
                    Start = start,
                    End = end,
                    Notes = trimmedNotes
                };

                var (ok, err) = await _bookingService.TryUpdateReservationAsync(updated);
                if (!ok)
                {
                    await _dialogs.AlertAsync("Could not update", err ?? "Please try a different time.");
                    return;
                }

                var detail =
                    $"{updated.RoomDisplayName}\n" +
                    $"{updated.Start:MMM d, yyyy · h:mm tt} – {updated.End:h:mm tt}";
                await _navigator.PushAsync(new BookingResultPage(BookingResultKind.Updated, detail));
            }
            else
            {
                var created = new Reservation
                {
                    RoomId = _room.Id,
                    RoomDisplayName = _room.DisplayName,
                    Start = start,
                    End = end,
                    Notes = trimmedNotes
                };

                var (ok, err) = await _bookingService.TryAddReservationAsync(created);
                if (!ok)
                {
                    await _dialogs.AlertAsync("Could not book", err ?? "Please try a different time.");
                    return;
                }

                await _navigator.PushAsync(new ConfirmationPage(created));
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}
