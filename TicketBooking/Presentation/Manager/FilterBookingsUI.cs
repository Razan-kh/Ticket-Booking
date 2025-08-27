using System;
using TicketBooking.Models;
using TicketBooking.Service;
using TicketBooking.Service;

namespace TicketBooking.Presentation;

public class FilterBookingsUI
{
    private readonly BookingService _bookingService;

    public FilterBookingsUI(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public void ShowFilterBookingsMenu()
    {
        BookingsFilter bookingsFilter = FilterBookingsMenu();
        var filteredBookings = _bookingService.FilterBookings(bookingsFilter);
        PrintBookings(filteredBookings);
    }

    public static BookingsFilter FilterBookingsMenu()
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

        BookingsFilter filterBookings = new()
        {
            ArrivalAirport = arrivalAirport,
            DepartureAirport = departureAirport,
            DestinationCountry = destinationCountry,
            DepartureCountry = departureCountry,
            FlightId = flightId,
            Price = price,
            PassengerId = passengerId,
            FlightClass = flightClass,
            DepartureDate = departureDate
        };

        return filterBookings;
    }
    
    public static void PrintBookings(List<Booking> bookings)
    {
        Console.WriteLine($"\n==== Found {bookings.Count} Booking(s) ====\n");
        foreach (var booking in bookings)
        {
            Console.WriteLine(booking);
        }
    }
}

