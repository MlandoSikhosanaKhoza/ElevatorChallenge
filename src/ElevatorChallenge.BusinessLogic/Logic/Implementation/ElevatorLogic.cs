using ElevatorChallenge.Services.Hubs;
using ElevatorChallenge.Shared.Enums;
using ElevatorChallenge.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.BusinessLogic
{
    public class ElevatorLogic : IElevatorLogic
    {
        private IBuildingStorageProvider _storageProvider;
        private IHubContext<ElevatorStatusHub, IElevatorStatusHub> _elevaterHubContext;
        public ElevatorLogic(IBuildingStorageProvider storageProvider, IHubContext<ElevatorStatusHub, IElevatorStatusHub> elevaterHubContext)
        {
            _storageProvider    = storageProvider;
            _elevaterHubContext = elevaterHubContext;
        }

        public Task AddPeopleToElevatorAsync(Elevator? elevator, int passengers)
        {
            if (elevator == null)
            {
                
                throw new ArgumentNullException(nameof(elevator));
            }

            if (passengers < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(passengers), "Number of passengers cannot be negative.");
            }

            if (elevator.PassengerCount + passengers > elevator.MaxPassengers)
            {
                throw new InvalidOperationException("Loading passengers would exceed elevator capacity.");
            }

            elevator.PassengerCount += passengers;
            _elevaterHubContext.Clients.All.BroadcastStatus(elevator.Id, elevator.CurrentFloor, elevator.PassengerCount);
            
            return Task.CompletedTask;
        }

        public async Task RelocateElevatorToFloorAsync(Elevator? elevator, int destinationFloor)
        {
            try
            {
                elevator.IsMoving = true;

                elevator.Status = DetectElevatorStatus(elevator, destinationFloor);


                while (elevator.CurrentFloor != destinationFloor)
                {

                    switch (elevator.Status)
                    {
                        case ElevatorStatus.MovingUp:
                            elevator.CurrentFloor += 1;
                            break;
                        case ElevatorStatus.MovingDown:
                            elevator.CurrentFloor -= 1;
                            break;
                    }
                    await _elevaterHubContext.Clients.All.BroadcastStatus(elevator.Id, elevator.CurrentFloor, elevator.PassengerCount);

                    await Task.Delay(1000);
                }

                elevator.IsMoving = false;
                elevator.Status = ElevatorStatus.Stationary;
            }
            catch (NullReferenceException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public Task RemovePeopleFromElevatorAsync(Elevator? elevator, int passengers)
        {
            if (passengers < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(passengers), "Passengers can't be negative.");
            }

            if (elevator.PassengerCount - passengers < 0)
            {
                throw new InvalidOperationException("Removing passengers will have a negative passenger count.");
            }

            if (elevator == null)
            {
                throw new ArgumentNullException(nameof(elevator));
            }

            

            elevator.PassengerCount -= passengers;
            _elevaterHubContext.Clients.All.BroadcastStatus(elevator.Id, elevator.CurrentFloor, elevator.PassengerCount);
            return Task.CompletedTask;
        }

        private ElevatorStatus DetectElevatorStatus(Elevator elevator, int destinationFloor)
        {
            if (elevator.CurrentFloor < destinationFloor)
            {
                return ElevatorStatus.MovingUp;
            }else if (elevator.CurrentFloor > destinationFloor)
            {
                return ElevatorStatus.MovingDown;
            }
            else
            {
                return ElevatorStatus.Stationary;
            }
        }
    }
}
