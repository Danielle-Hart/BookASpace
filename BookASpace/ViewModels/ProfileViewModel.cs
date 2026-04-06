using BookASpace.Models;
using BookASpace.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookASpace.ViewModels;

public partial class ProfileViewModel : ObservableObject
{
    private readonly ProfileService _profileService;
    private readonly IDialogService _dialogs;

    public ProfileViewModel(ProfileService profileService, IDialogService dialogs)
    {
        _profileService = profileService;
        _dialogs = dialogs;
    }

    [ObservableProperty]
    private string mNumber = string.Empty;

    [ObservableProperty]
    private string ucEmail = string.Empty;

    [ObservableProperty]
    private bool isBusy;

    [RelayCommand]
    public async Task LoadAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var profile = await _profileService.GetProfileAsync();
            MNumber = profile.MNumber;
            UcEmail = profile.UcEmail;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (IsBusy)
            return;

        try
        {
            IsBusy = true;
            var profile = new UserProfile
            {
                MNumber = MNumber.Trim(),
                UcEmail = UcEmail.Trim()
            };
            await _profileService.SaveProfileAsync(profile);
            await _dialogs.AlertAsync("Saved", "Your UC account details were saved on this device.");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
