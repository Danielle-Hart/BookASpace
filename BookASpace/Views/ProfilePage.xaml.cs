using BookASpace.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace BookASpace.Views;

public partial class ProfilePage : ContentPage
{
    public ProfilePage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<ProfileViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ProfileViewModel vm)
            await vm.LoadCommand.ExecuteAsync(null);
    }
}
