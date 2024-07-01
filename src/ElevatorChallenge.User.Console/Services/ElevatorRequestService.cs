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
        public async Task<(bool IsValid,List<string> Errors)> RequestElevatorAsync(int currentFloor,int destinationFloor,int passengerCount)
        {
            try
            {
                bool isValid = false;
                List<string> errors = new List<string>();
                HttpClient client = new HttpClient();
                string requestUrl = $"{ConsoleConstants.URL}/api/Building/RequestElevator?currentFloor={currentFloor}&destinationFloor={destinationFloor}&passengers={passengerCount}";
                client.BaseAddress = new Uri(requestUrl);
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    return (true, new List<string>());
                }
                else
                {
                    isValid = false;
                    errors = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync() ?? "[]") ?? new List<string>();
                }
                return (isValid, errors);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Please check if the API is a start up project",ex);
            }
        }
    }
}
