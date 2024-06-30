using ElevatorChallenge.Shared.Factory;
using ElevatorChallenge.Shared.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.BusinessLogic
{
    /// <summary>
    /// Singleton storage class
    /// </summary>
    public class BuildingStorageProvider : IBuildingStorageProvider
    {
        private readonly Building       _building;
        private readonly IConfiguration _configuration;
        
        public BuildingStorageProvider(IConfiguration configuration)
        {
            _configuration = configuration??throw new ArgumentNullException("Please inject the IConfiguration");
            _building      = BuildingFactory.CreateBuilding(configuration);
        }

        public Building GetBuilding()
        {
            return _building;
        }
    }
}
