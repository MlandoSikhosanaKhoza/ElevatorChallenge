using ElevatorChallenge.Shared.Models;
using ElevatorChallenge.Status.Console.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Status.Console.Services
{
    public class BuildingHttpService
    {
        public async Task<Building?> GetBuildingAsync()
        {
            HttpClient client = new HttpClient();
            string requestUrl = $"{ConsoleConstants.URL}/api/Building/GetBuilding";
            client.BaseAddress = new Uri(requestUrl);
            HttpResponseMessage response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                Building building = JsonConvert.DeserializeObject<Building>(await response.Content.ReadAsStringAsync());
                return building;
            }
            return null;
        }
    }
}
