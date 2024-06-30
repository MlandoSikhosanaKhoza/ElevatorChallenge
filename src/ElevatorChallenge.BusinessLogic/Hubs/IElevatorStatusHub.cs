using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Services.Hubs
{
    public interface IElevatorStatusHub
    {
        Task BroadcastStatus(int elevatorId, int currentFloor, int passengerCount);
    }
}
