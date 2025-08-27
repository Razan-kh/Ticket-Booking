using System;
using TicketBooking.Repository;
using TicketBooking.Service;

namespace TicketBooking.Presentation;

class ManagerInterface
{
    public static ManagerOptions PrintPassengerMenu()
    {
      while (true)
        {
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine(@"1. Filter Bookings
2. Upload and Validate Flights File
3. Generate Dynamic Validation Info"); 
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
    
    public static void UploadFile(FlightService flightService)
    {
        Console.Write("Enter path to the flight CSV file: ");
        string? csvPath = Console.ReadLine();
        if (string.IsNullOrEmpty(csvPath))
        {
            Console.WriteLine("Invalid Name");
            return;
        }
        var errors = flightService.BatchUploadFlights(csvPath);
        if (errors.Count != 0)
        {
            Console.WriteLine("Validation Errors:");
            foreach (var err in errors)
                Console.WriteLine(err);
        }
        else
        {
            Console.WriteLine("No errors found");
        }
    }
}