namespace ElevatorChallenge.Services.Hubs
{
    public interface IPassengerStatusHub
    {
        Task BroadcastPassengers(int currentFloor, string passengerJson);
    }
}