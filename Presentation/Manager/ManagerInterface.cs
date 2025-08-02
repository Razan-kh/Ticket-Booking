namespace Ticket_Booking.Presentation;

using System;

class ManagerInterface
{
    public static ManagerOptions PrintPassengerMenu()
    {
      while (true)
        {
            Console.WriteLine("=== Main Menu ===");
            System.Console.WriteLine(@"1. Upload and Validate Flights File"); 
            string? consoleChoice = Console.ReadLine();
            if (!int.TryParse(consoleChoice, out int numericChoice))
            {
                Console.WriteLine("Invalid option. Please try again.");
                continue;
            }
            if (!Enum.TryParse<ManagerOptions>(consoleChoice, out var choice))
            {
                Console.WriteLine("Invalid manager option selected.");
                continue;
            }

            return choice;
        }
    }
}