namespace Ticket_Booking.Presentation;

class PassengerInterface
{
    public static PassengerOptions PrintPassengerMenu()
    {
        while (true)
        {
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine(@"1. Search for Available Flights
2. Book a Flight
3. View Personal Bookings");
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
    public static void PersonalBookings(BookingService bookingService)
    {
        Console.WriteLine("Enter Your Id");
        var passengerId = Console.ReadLine();
        if(string.IsNullOrEmpty(passengerId))
            Console.WriteLine("Please enter a valid Id");
        var bookings = bookingService.GetBookingsForPassenger(passengerId!);
        foreach (var b in bookings)
            Console.WriteLine(b);
    }
}