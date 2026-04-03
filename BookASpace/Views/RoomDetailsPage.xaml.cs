using BookASpace.Models;

namespace BookASpace.Views;

public partial class RoomDetailsPage : ContentPage
{
    private Room _selectedRoom;

    public RoomDetailsPage(Room room)
    {
        InitializeComponent();
        _selectedRoom = room;

        RoomNameLabel.Text = room.DisplayName;
        DescriptionLabel.Text = $"Description: {room.Description}";
        CapacityLabel.Text = $"Capacity: {room.Capacity}";
        WhiteboardLabel.Text = $"Whiteboard: {(room.HasWhiteboard ? "Yes" : "No")}";
        OutletsLabel.Text = $"Outlets: {(room.HasOutlets ? "Yes" : "No")}";
    }

    private async void OnRoomClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Booked!", "Room Successfully Booked!", "OK");
    }
}