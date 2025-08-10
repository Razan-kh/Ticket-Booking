using System.Globalization;
using Ticket_Booking.Models;

namespace Ticket_Booking.Repository;

public class FlightRepository
{
    private readonly string _filePath;
    private readonly List<Flight> _flights;

    public FlightRepository(string filePath)
    {
        _filePath = filePath;
        _flights = ParseFile(_filePath);
    }

    public List<Flight> GetAllFlights() => _flights;

    public List<Flight> SearchFlights(FlightFilter filter)
    {
        return _flights.Where(f =>
            (string.IsNullOrEmpty(filter.DepartureCountry) || f.DepartureCountry.Equals(filter.DepartureCountry, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(filter.DestinationCountry) || f.DestinationCountry.Equals(filter.DestinationCountry, StringComparison.OrdinalIgnoreCase)) &&
            (!filter.DepartureDate.HasValue || f.DepartureDate.Date == filter.DepartureDate.Value.Date) &&
            (string.IsNullOrEmpty(filter.DepartureAirport) || f.DepartureAirport.Equals(filter.DepartureAirport, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(filter.ArrivalAirport) || f.ArrivalAirport.Equals(filter.ArrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
            (!filter.MaxPrice.HasValue ||
                (filter.ClassType.HasValue &&
                f.Prices.ContainsKey(filter.ClassType.Value) &&
                f.Prices[filter.ClassType.Value] <= filter.MaxPrice.Value))
        ).ToList();
    }

    public static List<Flight> ParseFile(string filePath)
    {
        List<Flight> flights = [];

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file at path '{filePath}' was not found.");
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length < 11) continue;
            if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var departureDate))
                throw new FormatException($"Invalid date format in: {parts[3]}");

            if (!int.TryParse(parts[6], out var seatsEconomy) ||
                !int.TryParse(parts[7], out var seatsBusiness) ||
                !int.TryParse(parts[8], out var seatsFirst))
                throw new FormatException("Invalid seat count.");

            if (!double.TryParse(parts[9], out var priceEconomy) ||
                !double.TryParse(parts[10], out var priceBusiness) ||
                !double.TryParse(parts[11], out var priceFirst))
                throw new FormatException("Invalid price format.");

            var flight = new Flight
            {
                Id = parts[0],
                DepartureCountry = parts[1],
                DestinationCountry = parts[2],
                DepartureDate = departureDate,
                DepartureAirport = parts[4],
                ArrivalAirport = parts[5],
                AvailableSeats = new Dictionary<FlightClass, int>
                {
                    { FlightClass.Economy, seatsEconomy },
                    { FlightClass.Business, seatsBusiness },
                    { FlightClass.FirstClass, seatsFirst }
                },
                Prices = new Dictionary<FlightClass, double>
                {
                    { FlightClass.Economy, priceEconomy },
                    { FlightClass.Business, priceBusiness },
                    { FlightClass.FirstClass, priceFirst }
                }
            };
            flights.Add(flight);
        }
        return flights;
    }
}
