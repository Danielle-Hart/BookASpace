using BookASpace.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BookASpace.Views;

public partial class AvailableRoomsPage : ContentPage
{
    public AvailableRoomsPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<AvailableRoomsViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AvailableRoomsViewModel vm)
            await vm.RefreshCommand.ExecuteAsync(null);
    }
}
