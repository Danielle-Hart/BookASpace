using System.Collections.ObjectModel;
using BookASpace.Models;
using BookASpace.Services;
using BookASpace.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookASpace.ViewModels;

public partial class AvailableRoomsViewModel : ObservableObject
{
    private readonly RoomService _roomService;
    private readonly BookingService _bookingService;
    private readonly IAppNavigator _navigator;

    public AvailableRoomsViewModel(
        RoomService roomService,
        BookingService bookingService,
        IAppNavigator navigator)
    {
        _roomService = roomService;
        _bookingService = bookingService;
        _navigator = navigator;
        Buildings = new ObservableCollection<string>();
        FilteredRooms = new ObservableCollection<Room>();
    }

    public ObservableCollection<string> Buildings { get; }

    [ObservableProperty]
    private string selectedBuilding = "All";

    [ObservableProperty]
    private bool requireWhiteboard;

    [ObservableProperty]
    private bool requireOutlets;

    [ObservableProperty]
    private bool onlyAvailableNow;

    [ObservableProperty]
    private bool isBusy;

    public ObservableCollection<Room> FilteredRooms { get; }

    partial void OnSelectedBuildingChanged(string value) => _ = ApplyFiltersAsync();

    partial void OnRequireWhiteboardChanged(bool value) => _ = ApplyFiltersAsync();

    partial void OnRequireOutletsChanged(bool value) => _ = ApplyFiltersAsync();

    partial void OnOnlyAvailableNowChanged(bool value) => _ = ApplyFiltersAsync();

    [RelayCommand]
    public async Task RefreshAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var rooms = _roomService.GetRooms();
            var distinctBuildings = rooms
                .Select(r => r.Building)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(b => b)
                .ToList();

            Buildings.Clear();
            Buildings.Add("All");
            foreach (var b in distinctBuildings)
                Buildings.Add(b);

            if (!Buildings.Contains(SelectedBuilding))
                SelectedBuilding = "All";

            await ApplyFiltersAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task ApplyFiltersAsync()
    {
        var all = _roomService.GetRooms();
        var reservations = await _bookingService.GetReservationsAsync();
        var now = DateTime.Now;

        IEnumerable<Room> query = all;

        if (!string.Equals(SelectedBuilding, "All", StringComparison.OrdinalIgnoreCase))
            query = query.Where(r => string.Equals(r.Building, SelectedBuilding, StringComparison.OrdinalIgnoreCase));

        if (RequireWhiteboard)
            query = query.Where(r => r.HasWhiteboard);

        if (RequireOutlets)
            query = query.Where(r => r.HasOutlets);

        if (OnlyAvailableNow)
            query = query.Where(r => _bookingService.IsRoomFreeAtNow(reservations, r.Id, now));

        var list = query
            .OrderBy(r => r.Building)
            .ThenBy(r => r.RoomNumber)
            .ToList();

        FilteredRooms.Clear();
        foreach (var room in list)
            FilteredRooms.Add(room);
    }

    [RelayCommand]
    private async Task ViewDetailsAsync(Room? room)
    {
        if (room == null)
            return;

        await _navigator.PushAsync(new RoomDetailsPage(room));
    }
}
