using ElevatorChallenge.Shared.Models;
using ElevatorChallenge.Status.Console.Constants;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Status.Console.Services
{
    public class ElevatorStorageService
    {
        public Building? Building {  get; set; }
        public HubConnection? ElevatorHubConnection { get; set; }
        public HubConnection? PassengerHubConnection { get; set; }
        
    }
}
