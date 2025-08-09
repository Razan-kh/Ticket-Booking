using System.Globalization;
using Ticket_Booking.Models;

namespace Ticket_Booking.Repository;

public class FlightRepository
{
    private readonly string _filePath;
    private List<Flight> _flights = [];

    public FlightRepository(string filePath)
    {
        _filePath = filePath;
        _flights = ParseFile(_filePath);
    }

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

    public List<Flight> ParseFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Invalid File path");
            return [];
        }
        var lines = File.ReadAllLines(filePath).Skip(1);

        foreach (var line in lines)
        {
            var parts = line.Split(',');

            if (parts.Length < 11) continue;

            if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var departureDate))
            {
                Console.WriteLine("Skipping flight due to invalid date format.");
                continue;
            }

            if (!int.TryParse(parts[6], out var seatsEconomy) ||
                !int.TryParse(parts[7], out var seatsBusiness) ||
                !int.TryParse(parts[8], out var seatsFirst))
            {
                Console.WriteLine("Skipping flight due to invalid format.");
                continue;
            }
            if (!double.TryParse(parts[9], out var priceEconomy) ||
                !double.TryParse(parts[10], out var priceBusiness) ||
                !double.TryParse(parts[11], out var priceFirst))
            {
                Console.WriteLine("Skipping flight due to invalid format.");
                continue;
            }
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

            _flights.Add(flight);
        }

        return _flights;
    }

    public Flight? GetFlightById(string flightId)
     => _flights.FirstOrDefault(f => f.Id == flightId);

    public void UpdateFlight(Flight updatedFlight)
    {
        var index = _flights.FindIndex(f => f.Id == updatedFlight.Id);
        if (index != -1)
        {
            _flights[index] = updatedFlight;
            SaveFlightsToCsv(_flights, _filePath);
        }
    }
    
    private static void SaveFlightsToCsv(List<Flight> flights, string path)
    {
        using var writer = new StreamWriter(path);
        writer.WriteLine("Id,DepartureCountry,DestinationCountry,DepartureDate,DepartureAirport,ArrivalAirport,EconomySeats,BusinessSeats,FirstClassSeats,EconomyPrice,BusinessPrice,FirstClassPrice");

        foreach (var flight in flights)
        {
            writer.WriteLine($"{flight.Id},{flight.DepartureCountry},{flight.DestinationCountry},{flight.DepartureDate.ToString("yyyy-MM-dd")},{flight.DepartureAirport},{flight.ArrivalAirport}," +
                $"{flight.AvailableSeats[FlightClass.Economy]},{flight.AvailableSeats[FlightClass.Business]},{flight.AvailableSeats[FlightClass.FirstClass]}," +
                $"{flight.Prices[FlightClass.Economy]},{flight.Prices[FlightClass.Business]},{flight.Prices[FlightClass.FirstClass]}");
        }
    }

    public List<Flight> GetAllFlights()
    => _flights;
}
