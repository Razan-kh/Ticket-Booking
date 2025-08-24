namespace TicketBooking.Presentation;

class PassengerInterface
{
    private readonly BookingService _bookingService;

    public PassengerInterface(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public static PassengerOptions PrintPassengerMenu()
    {
        while (true)
        {
            Console.WriteLine("=== Main Menu ===");
            System.Console.WriteLine(@"1. Search for Available Flights
2. Book a Flight
3. Modify a Booking
4. View Personal Bookings
5. Delete a Booking"); 
            string? consoleChoice = Console.ReadLine();
            if (!int.TryParse(consoleChoice, out int numericChoice))
            {
                Console.WriteLine("Invalid option. Please try again.");
                continue;
            }
            PassengerOptions choice = (PassengerOptions)numericChoice;
            return choice;
        }
    }

    public void UpdateBooking()
    {
        Console.Write("Enter Booking ID to modify: ");
        var modifyId = Console.ReadLine();
        Console.Write("Enter New Flight ID: ");
        var newFlight = Console.ReadLine();
        Console.Write("Enter New Class (Economy/Business/FirstClass): ");
        string? consoleClass = Console.ReadLine();
        if (!Enum.TryParse(consoleClass, out FlightClass newClass))
        {
            // Make the dafault class as Economy
            newClass = FlightClass.Economy;
        }
        _bookingService.UpdateOne(modifyId!, newFlight!, newClass);
        Console.WriteLine("Booking updated.");
    }
    public static void PersonalBookings(BookingService bookingService)
    {
        Console.WriteLine("Enter Your Id");
        var passengerId = Console.ReadLine();
        if (string.IsNullOrEmpty(passengerId))
            Console.WriteLine("Please enter a valid Id");
        var bookings = bookingService.GetBookingsForPassenger(passengerId!);
        foreach (var b in bookings)
            Console.WriteLine(b);
    }
}