using ElevatorChallenge.User.Console.Services;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        IServiceProvider services = ConfigureServices();
        ElevatorRequestService requestService = services.GetRequiredService<ElevatorRequestService>();
        string? input = "";
        do
        {
            PrintInstructions();
            try
            {
                Console.WriteLine("Enter the Current Floor, Destination Floor and Passengers in this format:");
                
                input = Console.ReadLine();
                if (input!=null && input.Split(' ').Length==3)
                {
                    int currentFloor = Convert.ToInt32(input.Split(" ")[0]);
                    int destinationFloor = Convert.ToInt32(input.Split(" ")[1]);
                    int passengers = Convert.ToInt32(input.Split(" ")[2]);
                    await requestService.RequestElevatorAsync(currentFloor, destinationFloor, passengers);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Input - Please read the instructions");
                PrintInstructions();
            }
        } while (!input.ToLower().Equals("q"));
        Console.ReadLine();
    }

    private static void PrintInstructions()
    {
        Console.WriteLine("Welcome to the Elevator For User");
        Console.WriteLine("Input the Current Floor, Destination Floor and Passengers in this format:");
        Console.WriteLine("Example 1: 10 2 6");
        Console.WriteLine("This represents 10 - Current Floor , 2 - Destination Floor, 6 - Passengers");
        Console.WriteLine("Example 2: 14 1 4");
        Console.WriteLine("This represents 14 - Current Floor , 1 - Destination Floor, 4 - Passengers");
        Console.WriteLine();
    }

    private static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<ElevatorRequestService>();
        return services.BuildServiceProvider();
    }
}