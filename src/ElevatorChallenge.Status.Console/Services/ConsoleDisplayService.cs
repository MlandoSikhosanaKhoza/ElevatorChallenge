using ElevatorChallenge.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Status.Console.Services
{
    public class ConsoleDisplayService
    {
        private readonly ElevatorStorageService _storageService;

        public ConsoleDisplayService(ElevatorStorageService storageService)
        {
            _storageService = storageService;
        }

        public void DisplayElevator()
        {
            if (_storageService.Building != null)
            {
                System.Console.Clear();
                Building building = _storageService.Building;
                Queue<ElevatorRequest> elevatorRequests = new Queue<ElevatorRequest>();
                for (int floorInvertedIndex = 0; floorInvertedIndex < building.FloorRequestDictionary.Count(); floorInvertedIndex++)
                {
                    int floor = building.NumOfFloors - floorInvertedIndex;
                    elevatorRequests = building.FloorRequestDictionary[floor];
                    //Floor Number
                    System.Console.Write(floor.ToString().PadLeft(4));
                    System.Console.Write("_|_");
                    foreach(Elevator elevator in building.Elevators.Take(building.NumOfElevators))
                    {
                        if (elevator.CurrentFloor == floor)
                        {
                            System.Console.Write(elevator.PassengerCount.ToString().PadLeft(4));
                            System.Console.Write("*|_");
                        }
                        else
                        {
                            System.Console.Write(0.ToString().PadLeft(4));
                            System.Console.Write("_|_");
                        }
                    }
                    foreach(ElevatorRequest elevatorRequest in elevatorRequests)
                    {
                        System.Console.Write($" {elevatorRequest.Passengers} peep/s | floor {elevatorRequest.DestinationFloor} ");
                    }
                    System.Console.WriteLine();
                }
            }
            
        }
    }
}
