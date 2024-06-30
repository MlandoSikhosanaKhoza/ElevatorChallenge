namespace ElevatorChallenge.Shared.Models
{
    public class ElevatorRequest
    {
        public int CurrentFloor     { get; }
        public int DestinationFloor { get; }
        public int Passengers       { get; }

        public ElevatorRequest(int CurrentFloor, int DestinationFloor, int Passengers)
        {
            this.CurrentFloor     = CurrentFloor;
            this.DestinationFloor = DestinationFloor;
            this.Passengers       = Passengers;
        }
    }
}