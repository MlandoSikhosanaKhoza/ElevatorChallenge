using ElevatorChallenge.Shared.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Shared.Factory
{
    public static class BuildingFactory
    {
        public static Building CreateBuilding(IConfiguration configuration)
        {
            try
            {
                var numberOfElevators = int.Parse(configuration["BuildingConfig:NumOfElevators"] ?? string.Empty);
                var numberOfFloors = int.Parse(configuration["BuildingConfig:NumOfFloors"] ?? string.Empty);
                var maxPassenger = int.Parse(configuration["BuildingConfig:MaxPassengers"] ?? string.Empty);

                return new Building(numberOfElevators, numberOfFloors, maxPassenger);
            }
            catch (FormatException ex)
            {
                throw new FormatException("Check that the building config settings are in the correct format",ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Issues with the building config settings",ex);
            }
        }
    }
}
