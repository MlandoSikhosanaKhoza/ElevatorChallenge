using ElevatorChallenge.Shared.Models;
using ElevatorChallenge.User.Console.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.User.Console.Services
{
    public class ElevatorRequestService
    {
        public async Task<bool> RequestElevatorAsync(int currentFloor,int destinationFloor,int passengerCount)
        {
            HttpClient client = new HttpClient();
            string requestUrl = $"{ConsoleConstants.URL}/api/Building/RequestElevator?currentFloor={currentFloor}&destinationFloor={destinationFloor}&passengers={passengerCount}";
            client.BaseAddress = new Uri(requestUrl);
            HttpResponseMessage response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
