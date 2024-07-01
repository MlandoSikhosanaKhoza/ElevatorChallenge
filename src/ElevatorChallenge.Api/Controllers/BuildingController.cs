using ElevatorChallenge.BusinessLogic;
using ElevatorChallenge.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElevatorChallenge.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BuildingController : ControllerBase
    {
        private IBuildingLogic _buildingLogic;
        private IBuildingStorageProvider _storageProvider;

        public BuildingController(IBuildingLogic buildingLogic, IBuildingStorageProvider storageProvider)
        {
            _buildingLogic   = buildingLogic;
            _storageProvider = storageProvider;
        }

        [HttpGet]
        [Produces("application/json", Type = typeof(Building))]
        public IActionResult GetBuilding()
        {
            return Ok(_storageProvider.GetBuilding());
        }

        [HttpGet]
        [Produces("application/json")]
        public IActionResult RequestElevator(int currentFloor, int destinationFloor, int passengers)
        {
            (bool isValid,List<string> errors) = _buildingLogic.ReviewElevator(currentFloor, destinationFloor, passengers);
            if (isValid)
            {
                Task task = _buildingLogic.SummonElevatorAsync(currentFloor, destinationFloor, passengers);
            }
            else
            {
                return BadRequest(errors);
            }
            return Ok();
        }
    }
}
