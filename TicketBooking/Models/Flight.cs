using System.ComponentModel.DataAnnotations;

namespace TicketBooking.Models;

public class Flight
{
    [Required(ErrorMessage = "Flight ID is required.")]
    public required string Id { get; set; }

    [Required(ErrorMessage = "Departure country is required.")]
    public required string DepartureCountry { get; set; }

    [Required]
    public required string DestinationCountry { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [FutureDate(ErrorMessage = "Departure date must be today or in the future.")]
    public required DateTime DepartureDate { get; set; }

    [Required]
    public required string DepartureAirport { get; set; }

    [Required]
    public required string ArrivalAirport { get; set; }

    [Required]
    public required Dictionary<FlightClass, int> AvailableSeats { get; set; }

    [Required]
    public required Dictionary<FlightClass, double> Prices { get; set; }
    
    public override string ToString()
    {
        return $"ID: {Id} | {DepartureCountry} ➡ {DestinationCountry} on {DepartureDate:yyyy-MM-dd}\n" +
            $"Airport: {DepartureAirport} ➝ {ArrivalAirport}";
    }
}