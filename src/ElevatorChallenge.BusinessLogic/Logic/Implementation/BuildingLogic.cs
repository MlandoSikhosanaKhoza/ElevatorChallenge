using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorChallenge.BusinessLogic;
using ElevatorChallenge.Services.Hubs;
using ElevatorChallenge.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace ElevatorChallenge.BusinessLogic
{
    public class BuildingLogic : IBuildingLogic
    {
        private readonly IBuildingStorageProvider _storageProvider;
        private readonly IElevatorLogic _elevatorLogic;
        private readonly IHubContext<PassengerStatusHub, IPassengerStatusHub> _passengerHubContext;
        public BuildingLogic(IBuildingStorageProvider storageProvider, IElevatorLogic elevatorLogic, IHubContext<PassengerStatusHub, IPassengerStatusHub> passengerHubContext)
        {
            _storageProvider     = storageProvider;
            _elevatorLogic       = elevatorLogic;
            _passengerHubContext = passengerHubContext;
        }

        public async Task SummonElevatorAsync(int currentFloor, int destinationFloor, int passengers, bool isBackloggedRequest = false)
        {
            try
            {
                Building building = _storageProvider.GetBuilding();
                Elevator? nearestElevator = FindClosestElevator(currentFloor);

                if (!isBackloggedRequest)
                {
                    building.FloorRequestDictionary[currentFloor].Enqueue(new ElevatorRequest(currentFloor, destinationFloor, passengers));
                    await _passengerHubContext.Clients.All.BroadcastPassengers(currentFloor, JsonConvert.SerializeObject(building.FloorRequestDictionary[currentFloor]));
                }

                if (nearestElevator != null)
                {
                    await SendElevatorAsync(nearestElevator, currentFloor, destinationFloor, passengers);
                    await HandleQueuedRequestsAsync();
                }
                else
                {
                    //_building.FloorRequestDictionary[currentFloor].Peek()
                    QueueElevatorRequest(currentFloor, destinationFloor, passengers);
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public (bool IsValid,List<string> Errors) ReviewElevator(int currentFloor,int destinationFloor,int passengerCount)
        {
            Building building   = _storageProvider.GetBuilding();
            bool isValid        = true;
            List<string> errors = new List<string>();
            if(currentFloor < 0 || destinationFloor < 0)
            {
                isValid = false;
                errors.Add("Current floor and Destination floor cannot be below zero");
            }
            if(currentFloor == destinationFloor)
            {
                isValid = false;
                errors.Add("Current floor and Destination floor cannot be equal");
            }
            if(currentFloor > building.NumOfFloors || destinationFloor > building.NumOfFloors)
            {
                isValid = false;
                errors.Add($"Current floor and Destination floor cannot be above {building.NumOfFloors}");
            }
            if(!(0 < passengerCount && passengerCount <= building.MaxPassengers))
            {
                isValid = false;
                errors.Add($"Passengers need to be between 1-{building.MaxPassengers}");
            }
            return (isValid, errors);
        }

        private Elevator? FindClosestElevator(int CurrentFloor)
        {
            Building building = _storageProvider.GetBuilding();
            return building.Elevators.Where(e => !e.IsMoving).OrderBy(e => Math.Abs(e.CurrentFloor - CurrentFloor))
                .FirstOrDefault();
        }

        private async Task SendElevatorAsync(Elevator elevator, int currentFloor, int destinationFloor, int passengers)
        {
            Building building = _storageProvider.GetBuilding();
            await _elevatorLogic.RelocateElevatorToFloorAsync(elevator, currentFloor);
            building.FloorRequestDictionary[currentFloor].Dequeue();
            await _passengerHubContext.Clients.All.BroadcastPassengers(currentFloor, JsonConvert.SerializeObject(building.FloorRequestDictionary[currentFloor]));
            await _elevatorLogic.AddPeopleToElevatorAsync(elevator, passengers);
            await _elevatorLogic.RelocateElevatorToFloorAsync(elevator, destinationFloor);
            await _elevatorLogic.RemovePeopleFromElevatorAsync(elevator, passengers);
        }

        private void QueueElevatorRequest(int currentFloor, int destinationFloor, int passengers)
        {
            Building building = _storageProvider.GetBuilding();
            building.BacklogElevatorRequestQueue.Enqueue(new ElevatorRequest(currentFloor, destinationFloor, passengers));
            //_logger.LogInformation("Elevator request queued as all elevators are busy.");
        }

        private async Task HandleQueuedRequestsAsync()
        {
            Building building = _storageProvider.GetBuilding();

            while (building.BacklogElevatorRequestQueue.Count > 0)
            {
                var request = building.BacklogElevatorRequestQueue.Dequeue();
                //_logger.LogInformation("Handling queued elevator request.");
                await SummonElevatorAsync(request.CurrentFloor, request.DestinationFloor, request.Passengers, true);
            }
        }
    }
}
