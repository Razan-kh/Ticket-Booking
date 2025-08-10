namespace Ticket_Booking.Passenger;

class PassengerInterface
{
    public static PassengerOptions PrintPassengerMenu()
    {
        while (true)
        {
            Console.WriteLine("=== Main Menu ===");
            System.Console.WriteLine("1. Search for Available Flights");
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
}