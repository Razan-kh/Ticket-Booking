namespace Ticket_Booking.Models;

    public class Flight
    {
        public required string Id { get; set; }
        public required string DepartureCountry { get; set; }
        public required string DestinationCountry { get; set; }
        public required DateTime DepartureDate { get; set; }
        public required string DepartureAirport { get; set; }
        public required string ArrivalAirport { get; set; }
        public required Dictionary<FlightClass, int> AvailableSeats { get; set; }
        public required Dictionary<FlightClass, double> Prices { get; set; }
    }

