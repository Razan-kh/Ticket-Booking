namespace Ticket_Booking.Presentation;

using System;

public class FilterBookings
{
    private readonly BookingService _bookingService;

    public FilterBookings(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public void ShowFilterBookingsMenu()
    {
        Console.Clear();
        Console.WriteLine("==== Filter Bookings ====");

        Console.Write("Enter Flight ID (or leave blank): ");
        string? flightId = Console.ReadLine();

        Console.Write("Enter Price (or leave blank): ");
        string? priceInput = Console.ReadLine();
        double? price = string.IsNullOrWhiteSpace(priceInput) ? null : double.Parse(priceInput);

        Console.Write("Enter Departure Country (or leave blank): ");
        string? departureCountry = Console.ReadLine();

        Console.Write("Enter Destination Country (or leave blank): ");
        string? destinationCountry = Console.ReadLine();

        Console.Write("Enter Departure Date (yyyy-MM-dd) (or leave blank): ");
        string? dateInput = Console.ReadLine();
        DateTime? departureDate = string.IsNullOrWhiteSpace(dateInput) ? null : DateTime.Parse(dateInput);

        Console.Write("Enter Departure Airport (or leave blank): ");
        string? departureAirport = Console.ReadLine();

        Console.Write("Enter Arrival Airport (or leave blank): ");
        string? arrivalAirport = Console.ReadLine();

        Console.Write("Enter Passenger ID (or leave blank): ");
        string? passengerId = Console.ReadLine();

        Console.Write("Enter Flight Class (Economy, Business, First) (or leave blank): ");
        string? classInput = Console.ReadLine();
        FlightClass? flightClass = null;

        if (!string.IsNullOrWhiteSpace(classInput) && Enum.TryParse(classInput, out FlightClass parsedClass))
        {
            flightClass = parsedClass;
        }

        var filteredBookings = _bookingService.FilterBookings(
            flightId,
            price,
            departureCountry,
            destinationCountry,
            departureDate,
            departureAirport,
            arrivalAirport,
            passengerId,
            flightClass
        );

        Console.WriteLine($"\n==== Found {filteredBookings.Count} Booking(s) ====\n");
        foreach (var booking in filteredBookings)
        {
            Console.WriteLine(booking);
        }
    }
}

