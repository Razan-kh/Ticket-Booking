namespace Ticket_Booking.Presentation;

using System;
using System.Globalization;
using Ticket_Booking.Models;
using Ticket_Booking.Repository;

public class FlightUI
{
    private readonly FlightService _service;

    public FlightUI(FlightService service)
    {
        _service = service;
    }

    public void Run()
    {
        Console.WriteLine("=== Search Flights ===");

        string depCountry = Prompt("Departure Country");
        string destCountry = Prompt("Destination Country");

        DateTime? depDate = PromptDate("Departure Date (yyyy-MM-dd)");
        string depAirport = Prompt("Departure Airport");
        string arrAirport = Prompt("Arrival Airport");

        FlightClass? selectedClass = PromptFlightClass("Flight Class (Economy/Business/FirstClass)");
        double? maxPrice = PromptDouble("Max Price");

        var results = _service.SearchFlights(depCountry, destCountry, depDate, depAirport, arrAirport, maxPrice, selectedClass);

        DisplayFlights(results, selectedClass);
    }

    private string Prompt(string label)
    {
        Console.Write($"{label}: ");
        return Console.ReadLine();
    }

    private DateTime? PromptDate(string label)
    {
        Console.Write($"{label}: ");
        string input = Console.ReadLine();
        if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
        {
            return parsed;
        }
        return null;
    }

    private double? PromptDouble(string label)
    {
        Console.Write($"{label}: ");
        string input = Console.ReadLine();
        if (double.TryParse(input, out double value))
        {
            return value;
        }
        return null;
    }

    private FlightClass? PromptFlightClass(string label)
    {
        Console.Write($"{label}: ");
        string input = Console.ReadLine();
        if (Enum.TryParse(input, out FlightClass flightClass))
        {
            return flightClass;
        }
        return null;
    }

    private void DisplayFlights(List<Flight> flights, FlightClass? selectedClass)
    {
        Console.WriteLine("\n--- Available Flights ---");

        if (flights.Count == 0)
        {
            Console.WriteLine("No flights found.");
            return;
        }

        foreach (var flight in flights)
        {
            Console.WriteLine($"ID: {flight.Id} | {flight.DepartureCountry} ➡ {flight.DestinationCountry} on {flight.DepartureDate:yyyy-MM-dd}");
            Console.WriteLine($"Airport: {flight.DepartureAirport} ➝ {flight.ArrivalAirport}");

            if (selectedClass.HasValue)
            {
                Console.WriteLine($"Class: {selectedClass} | Price: {flight.Prices[selectedClass.Value]} | Seats: {flight.AvailableSeats[selectedClass.Value]}");
            }

            Console.WriteLine("--------------------------------------------------");
        }
    }
}
