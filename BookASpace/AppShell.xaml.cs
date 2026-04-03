using BookASpace.Views;

namespace BookASpace;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(AvailableRoomsPage), typeof(AvailableRoomsPage));
        Routing.RegisterRoute(nameof(RoomDetailsPage), typeof(RoomDetailsPage));
        Routing.RegisterRoute(nameof(BookRoomPage), typeof(BookRoomPage));
        Routing.RegisterRoute(nameof(MyReservationsPage), typeof(MyReservationsPage));
        Routing.RegisterRoute(nameof(ConfirmationPage), typeof(ConfirmationPage));
    }
}