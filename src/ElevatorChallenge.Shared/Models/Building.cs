using ElevatorChallenge.Shared.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Shared.Models
{
    public class Building
    {
        public int NumOfElevators       { get; }
        public int NumOfFloors          { get; }
        public int MaxPassengers        { get; }
        public List<Elevator> Elevators { get; }
        public Queue<ElevatorRequest> BacklogElevatorRequestQueue             { get; }
        public Dictionary<int, Queue<ElevatorRequest>> FloorRequestDictionary { get; }

        public Building(int NumOfElevators, int NumOfFloors,  int MaxPassengers)
        {
            this.NumOfElevators         = NumOfElevators;
            this.NumOfFloors            = NumOfFloors;
            this.MaxPassengers          = MaxPassengers;
            
            this.Elevators                   = ElevatorFactory.GeneraterList(NumOfElevators, NumOfFloors, MaxPassengers);
            this.FloorRequestDictionary      = FloorRequestFactory.GenerateDictionary(NumOfFloors);
            this.BacklogElevatorRequestQueue = new Queue<ElevatorRequest>();
        }
    }
}
