using ElevatorChallenge.Shared.Models;
using ElevatorChallenge.Status.Console.Constants;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Status.Console.Services
{
    public class ElevatorRealTimeService
    {
        private readonly ElevatorStorageService _storageService;
        private readonly ConsoleDisplayService _displayService;
        public ElevatorRealTimeService(ElevatorStorageService storageService, ConsoleDisplayService displayService)
        {
            _storageService = storageService;
            _displayService = displayService;
        }

        public async Task InitializeRealTimeElevator()
        {
            _storageService.ElevatorHubConnection = new HubConnectionBuilder().WithUrl($"{ConsoleConstants.URL}/elevator/status").Build();
            _storageService.ElevatorHubConnection.On<int, int, int>("BroadcastStatus", (int elevatorId, int currentFloor, int passengerCount) =>
            {
                Elevator? elevator = _storageService.Building.Elevators.FirstOrDefault(e => e.Id == elevatorId);
                if(elevator != null)
                {
                    elevator.CurrentFloor   = currentFloor;
                    elevator.PassengerCount = passengerCount;
                }
                _displayService.DisplayElevator();
            });
            await _storageService.ElevatorHubConnection.StartAsync();
        }

        public async Task InitializeRealTimePassengerRequests()
        {
            _storageService.PassengerHubConnection = new HubConnectionBuilder().WithUrl($"{ConsoleConstants.URL}/passenger/status").Build();
            _storageService.PassengerHubConnection.On<int, string>("BroadcastPassengers", (int floorNumber, string passengerJson) =>
            {
                IEnumerable<ElevatorRequest>? elevatorRequests = JsonConvert.DeserializeObject<List<ElevatorRequest>>(passengerJson)??new List<ElevatorRequest>();
                _storageService.Building.FloorRequestDictionary[floorNumber].Clear();
                foreach(var elevatorRequest in elevatorRequests)
                {
                    _storageService.Building.FloorRequestDictionary[floorNumber].Enqueue(elevatorRequest);
                }
                _displayService.DisplayElevator();
            });
            await _storageService.PassengerHubConnection.StartAsync();
        }
    }
}
