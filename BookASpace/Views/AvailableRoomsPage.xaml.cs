using BookASpace.Models;
using BookASpace.Services;

namespace BookASpace.Views;

public partial class AvailableRoomsPage : ContentPage
{
    private readonly RoomService _roomService;
    private List<Room> _rooms;

    public AvailableRoomsPage()
    {
        InitializeComponent();

        _roomService = new RoomService();
        _rooms = _roomService.GetRooms();

        RoomsCollectionView.ItemsSource = _rooms;
    }

    private async void OnViewDetailsClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var room = button?.BindingContext as Room;

        if (room != null)
        {
            await Navigation.PushAsync(new RoomDetailsPage(room));
        }
    }
}