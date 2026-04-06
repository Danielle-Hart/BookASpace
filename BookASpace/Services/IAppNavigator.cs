namespace BookASpace.Services;

public interface IAppNavigator
{
    Task PushAsync(Page page);
    Task PopToRootAsync();
}
