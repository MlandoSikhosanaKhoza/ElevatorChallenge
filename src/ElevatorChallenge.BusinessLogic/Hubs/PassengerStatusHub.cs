using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Services.Hubs
{
    public class PassengerStatusHub : Hub<IPassengerStatusHub>
    {
        public Task BroadcastPassengers(int currentFloor, string passengerJson)
        {
            return Clients.All.BroadcastPassengers(currentFloor, passengerJson);
        }
    }
}
