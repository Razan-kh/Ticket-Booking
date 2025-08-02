namespace Ticket_Booking.Repository;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using Ticket_Booking.Models;

public class FlightRepository
{
    private readonly string _filePath;
    private List<Flight> _flights = [];
    public FlightRepository(string filePath)
    {
        _filePath = filePath;
    }
    public List<Flight> GetAllFlights()
    {
        _flights.Clear();
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"The file at path '{_filePath}' was not found.");
        var lines = File.ReadAllLines(_filePath).Skip(1);
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

            _flights.Add(flight);
        }

        return _flights;
    }

    public Flight? GetFlightById(string flightId)
    { 
        _flights = GetAllFlights();
        return _flights.FirstOrDefault(f => f.Id == flightId);
    }

    public void UpdateFlight(Flight updatedFlight)
    {
        var index = _flights.FindIndex(f => f.Id == updatedFlight.Id);
        if (index != -1)
        {
            _flights[index] = updatedFlight;
            SaveFlightsToCsv(_flights, _filePath);
        }
    }
    private void SaveFlightsToCsv(List<Flight> flights, string path)
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

}
