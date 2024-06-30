using ElevatorChallenge.Shared.Models;

namespace ElevatorChallenge.BusinessLogic
{
    public interface IBuildingStorageProvider
    {
        Building GetBuilding();
    }
}