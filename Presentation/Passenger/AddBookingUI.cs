using Ticket_Booking.Models;
using Ticket_Booking.Repository;

namespace Ticket_Booking.Presentation;

public class AddBookingUI
{
    private readonly FlightService _flightService;
    private readonly BookingService _bookingService;

    public AddBookingUI(FlightService flightService, BookingService bookingService)
    {
        _flightService = flightService;
        _bookingService = bookingService;
    }

    public void BookFlight()
    {
        Console.WriteLine("=== Book a Flight ===");
        string flightId = PromptFlight();
        Flight? flight = ValidateFlight(flightId);
        if (flight is null) return;
        DisplayAvailableClasses(flight);
        if (!TryGetFlightClass(out var selectedClass)) return;

        string? passengerId = PromptPassengerId();
        if (passengerId is null) return;

        if (string.IsNullOrEmpty(passengerId))
        {
            Console.WriteLine("Invalid Passenger Id");
            return;
        }
        _bookingService.BookFlight(passengerId, flightId, selectedClass);
    }

    public static string PromptFlight()
    {
        string? flightId;
        while (true)
        {
            Console.Write("Enter Flight ID: ");
            flightId = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(flightId))
            {
                return flightId;
            }
            Console.WriteLine("Flight ID cannot be empty.");
        }
    }
    
    private Flight? ValidateFlight(string flightId)
    {
        var flight = _flightService.GetFlightById(flightId);
        if (flight is null)
        {
            Console.WriteLine("Flight not found.");
            return null;
        }

        return flight;
    }

    private static void DisplayAvailableClasses(Flight flight)
    {
        Console.WriteLine("Available Classes:");
        foreach (var kvp in flight.AvailableSeats)
        {
            Console.WriteLine($"{kvp.Key} - {kvp.Value} seats | Price: {flight.Prices[kvp.Key]}");
        }
    }

    private static bool TryGetFlightClass(out FlightClass selectedClass)
    {
        Console.Write("Select class to book (Economy/Business/FirstClass): ");
        var inputClass = Console.ReadLine();

        if (!Enum.TryParse(inputClass, out selectedClass))
        {
            Console.WriteLine("Invalid class selection.");
            return false;
        }
        return true;
    }

    private static string? PromptPassengerId()
    {
        Console.Write("Enter your Passenger ID: ");
        var passengerId = Console.ReadLine();
        if (string.IsNullOrEmpty(passengerId))
        {
            Console.WriteLine("Invalid Passenger ID.");
            return null;
        }
        return passengerId;
    }
}