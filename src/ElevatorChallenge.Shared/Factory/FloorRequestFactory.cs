using ElevatorChallenge.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Shared.Factory
{
    public static class FloorRequestFactory
    {
        public static Dictionary<int, Queue<ElevatorRequest>> GenerateDictionary(int MaxFloors)
        {
            Dictionary<int, Queue<ElevatorRequest>> floorRequestDictionary = new Dictionary<int, Queue<ElevatorRequest>>();
            
            for (int floor = 0; floor <= MaxFloors; floor++)
            {
                floorRequestDictionary.Add(floor, new Queue<ElevatorRequest>());
            }

            return floorRequestDictionary;
        }
    }
}
