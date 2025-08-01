namespace Ticket_Booking.Repository;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using Ticket_Booking.Models;

public class FlightRepository
{
    private string filePath = "Files/Flights.csv";

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
}