using ElevatorChallenge.BusinessLogic;
using ElevatorChallenge.Shared.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevator.Challenge.Test.BusinessLogic
{
    public class BuildingStorageProviderTests
    {
        //SUT
        private readonly Mock<IBuildingStorageProvider> _buildingStorageProviderMock;
        private readonly Mock<IConfiguration> _configurationMock;
        
        private readonly BuildingStorageProvider _buildingStorageProvider;

        public BuildingStorageProviderTests()
        {
            _buildingStorageProviderMock = new Mock<IBuildingStorageProvider>();
            _configurationMock           = new Mock<IConfiguration>();

            _configurationMock.Setup(c => c["BuildingConfig:NumOfElevators"]).Returns("3");
            _configurationMock.Setup(c => c["BuildingConfig:NumOfFloors"]).Returns("20");
            _configurationMock.Setup(c => c["BuildingConfig:MaxPassengers"]).Returns("10");

            _buildingStorageProvider = new BuildingStorageProvider(_configurationMock.Object);
        }

        [Fact]
        public void BuildingStorageProvider_GetBuilding_ReturnsRequiredValues()
        {
            //Act
            Building building = _buildingStorageProvider.GetBuilding();
            //Assert
            building.Should().NotBeNull();
            building.FloorRequestDictionary.Count().Should().BePositive();
            building.Elevators.Count().Should().BePositive();
        }

        /// <summary>
        /// Test that the number of Elevators in the list is equivalent to the Elevators List count
        /// </summary>
        /// <param name="NumOfElevators"></param>
        /// <param name="NumOfFloors"></param>
        /// <param name="MaxPassengers"></param>
        [Theory]
        [InlineData(4,100,6)]
        [InlineData(3,50,10)]
        [InlineData(2,30,12)]
        [InlineData(5,200,20)]
        public void BuildingStorageProvider_GetBuilding_ReturnsMatchingElevators(int NumOfElevators, int NumOfFloors, int MaxPassengers)
        {
            Building building = new Building(NumOfElevators,NumOfFloors,MaxPassengers);
            //Act
            int elevatorListCount = building.Elevators.Count();
            //Assert
            elevatorListCount.Should().Be(NumOfElevators);
        }

        /// <summary>
        /// Test that the number of Floors in the list is equivalent to the Floor Dictionary Count
        /// </summary>
        /// <param name="NumOfElevators"></param>
        /// <param name="NumOfFloors"></param>
        /// <param name="MaxPassengers"></param>
        [Theory]
        [InlineData(4, 100, 6)]
        [InlineData(3, 50, 10)]
        [InlineData(2, 30, 12)]
        [InlineData(5, 200, 20)]
        public void BuildingStorageProvider_GetBuilding_ReturnsMatchingFloors(int NumOfElevators, int NumOfFloors, int MaxPassengers)
        {
            Building building = new Building(NumOfElevators, NumOfFloors, MaxPassengers);
            //Act
            int elevatorListCount = building.FloorRequestDictionary.Count();
            //Assert
            elevatorListCount.Should().Be(NumOfFloors+1);
        }
    }
}
