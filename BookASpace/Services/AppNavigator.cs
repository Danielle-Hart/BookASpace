namespace BookASpace.Services;

public sealed class AppNavigator : IAppNavigator
{
    public Task PushAsync(Page page) =>
        Shell.Current.Navigation.PushAsync(page);

    public Task PopToRootAsync() =>
        Shell.Current.Navigation.PopToRootAsync();
}
