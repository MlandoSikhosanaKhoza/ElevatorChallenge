using ElevatorChallenge.User.Console.Services;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        IServiceProvider services = ConfigureServices();
        ElevatorRequestService requestService = services.GetRequiredService<ElevatorRequestService>();
        string? input = "";
        PrintInstructions();
        do
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine("Enter the Current Floor, Destination Floor and Passengers:");
                
                input = Console.ReadLine();
                if (input!=null && input.Trim().Split(' ').Length == 3)
                {
                    int currentFloor     = Convert.ToInt32(input.Split(" ")[0]);
                    int destinationFloor = Convert.ToInt32(input.Split(" ")[1]);
                    int passengers       = Convert.ToInt32(input.Split(" ")[2]);

                    (bool isValid,List<string> errors) = await requestService.RequestElevatorAsync(currentFloor, destinationFloor, passengers);

                    if(!isValid) {
                        Console.WriteLine();
                        Console.WriteLine(string.Join(Environment.NewLine,errors));
                        Console.WriteLine();
                    }
                }
                else if(input == "i")
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    PrintInstructions();
                }
                else if(input != "q")
                {
                    Console.WriteLine();
                    Console.WriteLine("Invalid Data - Press i for instructions");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Input - Please read the instructions");
                Console.WriteLine();
                Console.WriteLine("Press i to view instructions");
                Console.WriteLine("Press q to quit");
                Console.WriteLine();
            }
        } while (!input.ToLower().Equals("q"));
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
        Console.WriteLine("Press i to view instructions");
        Console.WriteLine("Press q to quit");
        Console.WriteLine();
        Console.WriteLine();
    }

    private static IServiceProvider ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddSingleton<ElevatorRequestService>();
        return services.BuildServiceProvider();
    }
}