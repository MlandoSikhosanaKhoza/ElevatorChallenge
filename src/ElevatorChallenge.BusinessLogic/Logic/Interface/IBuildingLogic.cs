namespace ElevatorChallenge.BusinessLogic
{
    public interface IBuildingLogic
    {
        Task SummonElevatorAsync(int CurrentFloor, int DestinationFloor, int Passengers, bool IsBackloggedRequest = false);
        (bool IsValid, List<string> Errors) ReviewElevator(int currentFloor, int destinationFloor, int passengerCount);
    }
}