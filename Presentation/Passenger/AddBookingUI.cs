namespace Ticket_Booking.Presentation;

using System;
using Ticket_Booking.Models;
using Ticket_Booking.Repository;

public class BookingUI
{
    private readonly FlightService _flightService;
    private readonly BookingService _bookingService;

    public BookingUI(FlightService flightService, BookingService bookingService)
    {
        _flightService = flightService;
        _bookingService = bookingService;
    }

    public void Run()
    {
        Console.WriteLine("=== Book a Flight ===");

        Console.Write("Enter Flight ID: ");
        string flightId = Console.ReadLine();

        var flight = _flightService.GetFlightById(flightId);

        if (flight is null)
        {
            Console.WriteLine("Flight not found.");
            return;
        }

        Console.WriteLine("Available Classes:");
        foreach (var kvp in flight.AvailableSeats)
        {
            Console.WriteLine($"{kvp.Key} - {kvp.Value} seats | Price: {flight.Prices[kvp.Key]}");
        }

        Console.Write("Select class to book (Economy/Business/FirstClass): ");
        string inputClass = Console.ReadLine();

        if (!Enum.TryParse(inputClass, out FlightClass selectedClass))
        {
            Console.WriteLine("Invalid class selection.");
            return;
        }
        Console.Write("Enter your Passenger ID: ");
        string passengerId = Console.ReadLine();
         _bookingService.BookFlight(passengerId,flightId, selectedClass);
    }
}
