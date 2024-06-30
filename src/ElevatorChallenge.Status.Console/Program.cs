using ElevatorChallenge.Status.Console.Constants;
using ElevatorChallenge.Status.Console.Services;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        IServiceProvider services                       = ConfigureServices();
        ElevatorStorageService storageService           = services.GetRequiredService<ElevatorStorageService>();
        BuildingHttpService buildingHttpService         = services.GetRequiredService<BuildingHttpService>();
        ElevatorRealTimeService elevatorRealTimeService = services.GetRequiredService<ElevatorRealTimeService>();
        ConsoleDisplayService consoleDisplayService     = services.GetRequiredService<ConsoleDisplayService>();

        storageService.Building = await buildingHttpService.GetBuildingAsync();
        
        await elevatorRealTimeService.InitializeRealTimeElevator();
        await elevatorRealTimeService.InitializeRealTimePassengerRequests();
        
        consoleDisplayService.DisplayElevator();

        Console.ReadLine();
    }

    private static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<ElevatorStorageService>();
        services.AddTransient<ElevatorRealTimeService>();
        services.AddTransient<BuildingHttpService>();
        services.AddTransient<ConsoleDisplayService>();

        return services.BuildServiceProvider();
    }
}
