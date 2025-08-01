namespace Ticket_Booking.Repository;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using Ticket_Booking.Models;

public class FlightRepository
{
    private string filePath = "Files/Flights.csv";
    private List<Flight> _flights = new List<Flight>();
    public List<Flight> GetAllFlights()
    {
        _flights.Clear();
        if (!File.Exists(filePath))
            return _flights;

        var lines = File.ReadAllLines(filePath).Skip(1);

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
            SaveFlightsToCsv(_flights, "Files/Flights.csv");
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