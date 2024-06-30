using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Services.Hubs
{
    public class ElevatorStatusHub : Hub<IElevatorStatusHub>
    {
        public Task BroadcastStatus(int elevatorId,int currentFloor,int passengerCount)
        {
            return Clients.All.BroadcastStatus(elevatorId,  currentFloor,  passengerCount);
        }
    }
}
