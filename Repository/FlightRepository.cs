namespace Ticket_Booking.Repository;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using Ticket_Booking.Models;

public class FlightRepository
{
    private string _filePath = "Files/Flights.csv";

    public FlightRepository()
    {
        _flights = LoadFlightsFromCsv();
    }
    public List<Flight> GetAllFlights()
    {
        var flights = new List<Flight>();

        if (!File.Exists(filePath))
            return flights;

        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');

            if (parts.Length < 11) continue;

            Flight flight = new Flight
            {
                Id = parts[0],
                DepartureCountry = parts[1],
                DestinationCountry = parts[2],
                DepartureDate = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                DepartureAirport = parts[4],
                ArrivalAirport = parts[5],
                AvailableSeats = new Dictionary<FlightClass, int>
                {
                    { FlightClass.Economy, int.Parse(parts[6]) },
                    { FlightClass.Business, int.Parse(parts[7]) },
                    { FlightClass.FirstClass, int.Parse(parts[8]) }
                },
                Prices = new Dictionary<FlightClass, double>
                {
                    { FlightClass.Economy, double.Parse(parts[9]) },
                    { FlightClass.Business, double.Parse(parts[10]) },
                    { FlightClass.FirstClass, double.Parse(parts[11]) }
                }
            };

            flights.Add(flight);
        }

        return flights;
    }
    public Flight? GetFlightById(string flightId)
    {
        return _flights.FirstOrDefault(f => f.Id == flightId);
    }

    public void UpdateFlight(Flight flight)
    {
        var index = _flights.FindIndex(f => f.Id == flight.Id);
        if (index != -1)
        {
            _flights[index] = flight;
            SaveFlightsToCsv();
        }
    }
    public void SaveFlightsToCsv(List<Flight> flights)
    {
        var lines = new List<string>();

        foreach (var flight in flights)
        {
            string prices = string.Join(";", flight.Prices.Select(p => $"{p.Key}:{p.Value}"));
            string seats = string.Join(";", flight.AvailableSeats.Select(s => $"{s.Key}:{s.Value}"));

            string line = string.Join(",",
                flight.Id,
                flight.DepartureCountry,
                flight.DestinationCountry,
                flight.DepartureDate.ToString("yyyy-MM-dd"),
                flight.DepartureAirport,
                flight.ArrivalAirport,
                prices,
                seats
            );

            lines.Add(line);
        }

        File.WriteAllLines(_filePath, lines);
    }
}
