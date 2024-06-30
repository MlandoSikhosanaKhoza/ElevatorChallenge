using ElevatorChallenge.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorChallenge.Shared.Factory
{
    public static class ElevatorFactory
    {
        /// <summary>
        /// Prepopulate Elevators
        /// </summary>
        /// <param name="NumOfElevators"></param>
        /// <param name="MaxFloors"></param>
        /// <param name="MaxPassengers"></param>
        /// <returns></returns>
        public static List<Elevator> GeneraterList(int NumOfElevators, int MaxFloors, int MaxPassengers)
        {
            List<Elevator> elevators = new ();

            for (int i = 1; i <= NumOfElevators; i++)
            {
                elevators.Add(new Elevator(i, MaxFloors, MaxPassengers));
            }

            return elevators;
        }
    }
}
