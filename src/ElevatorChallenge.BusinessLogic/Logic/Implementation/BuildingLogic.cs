using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorChallenge.BusinessLogic;

namespace ElevatorChallenge.BusinessLogic
{
    public class BuildingLogic : IBuildingLogic
    {
        private readonly IBuildingStorageProvider _storageProvider;
        public BuildingLogic(IBuildingStorageProvider storageProvider)
        {
            _storageProvider = storageProvider;
        }


    }
}
