using ElevatorChallenge.Shared.Models;

namespace ElevatorChallenge.BusinessLogic
{
    public interface IElevatorLogic
    {
        Task RelocateElevatorToFloorAsync(Elevator? elevator, int destinationFloor);

        Task AddPeopleToElevatorAsync(Elevator? elevator, int passengers);

        Task RemovePeopleFromElevatorAsync(Elevator? elevator, int passengers);
    }
}