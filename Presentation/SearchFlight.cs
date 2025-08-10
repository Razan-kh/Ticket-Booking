using System;
using System.Globalization;
using Ticket_Booking.Models;
using Ticket_Booking.Repository;

namespace Ticket_Booking.Presentation;

public class SearchFlight
{
    private readonly FlightService _service;

    public SearchFlight(FlightService flightService)
    {
        _service = flightService;
    }
    
    public void Search()
    {
        Console.WriteLine("=== Search Flights ===");

        string? depCountry = Prompt("Departure Country");
        string? destCountry = Prompt("Destination Country");

        DateTime? depDate = PromptDate("Departure Date (yyyy-MM-dd)");
        string? depAirport = Prompt("Departure Airport");
        string? arrAirport = Prompt("Arrival Airport");

        FlightClass? selectedClass = PromptFlightClass("Flight Class (Economy/Business/FirstClass)");
        double? maxPrice = PromptDouble("Max Price");
        FlightFilter flightFilter = new()
        {
            DepartureCountry = depCountry,
            ArrivalAirport = arrAirport,
            ClassType = selectedClass,
            DepartureAirport = depAirport,
            DepartureDate = depDate,
            DestinationCountry = destCountry,
            MaxPrice = maxPrice
        };
        var results = _service.SearchFlights(flightFilter);
        DisplayFlights(results, selectedClass);
    }

    private static string? Prompt(string label)
    {
        Console.Write($"{label}: ");
        return Console.ReadLine();
    }

    private static DateTime? PromptDate(string label)
    {
        Console.Write($"{label}: ");
        string? input = Console.ReadLine();
        if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
        {
            return parsed;
        }
        return null;
    }

    private static double? PromptDouble(string label)
    {
        Console.Write($"{label}: ");
        string? input = Console.ReadLine();
        if (double.TryParse(input, out double value))
        {
            return value;
        }
        return null;
    }

    private static FlightClass? PromptFlightClass(string label)
    {
        Console.Write($"{label}: ");
        string? input = Console.ReadLine();
        if (Enum.TryParse(input, out FlightClass flightClass))
        {
            return flightClass;
        }
        return null;
    }

    private static void DisplayFlights(List<Flight> flights, FlightClass? selectedClass)
    {
        Console.WriteLine("\n--- Available Flights ---");
        if (flights.Count == 0)
        {
            Console.WriteLine("No flights found.");
            return;
        }
        foreach (var flight in flights)
        {
            Console.WriteLine(flight);

            if (selectedClass.HasValue)
            {
                Console.WriteLine($"Class: {selectedClass} | Price: {flight.Prices[selectedClass.Value]} | Seats: {flight.AvailableSeats[selectedClass.Value]}");
            }
            Console.WriteLine("--------------------------------------------------");
        }
    }
}
