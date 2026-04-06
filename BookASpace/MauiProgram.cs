using BookASpace.Services;
using BookASpace.ViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace BookASpace
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<RoomService>();
            builder.Services.AddSingleton<BookingService>();
            builder.Services.AddSingleton<ProfileService>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<IAppNavigator, AppNavigator>();

            builder.Services.AddTransient<AvailableRoomsViewModel>();
            builder.Services.AddTransient<MyReservationsViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();
            App.Services = app.Services;
            return app;
        }
    }
}
