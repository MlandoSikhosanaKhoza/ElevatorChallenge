using ElevatorChallenge.Shared.Enums;

namespace ElevatorChallenge.Shared.Models
{
    public class Elevator
    {
        public int Id                { get; }
        public int CurrentFloor      { get; set; }
        public int MaxFloor          { get; }
        public int PassengerCount    { get; set; }
        public int MaxPassengers     { get; }
        public bool IsMoving         { get; set; }
        public ElevatorStatus Status { get; set; }

        public Elevator(int Id, int MaxFloor, int MaxPassengers)
        {
            this.Id            = Id;
            this.MaxPassengers = MaxPassengers;
            this.MaxFloor      = MaxFloor;

            Status = ElevatorStatus.Stationary;
        }
    }
}