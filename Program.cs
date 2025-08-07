using Ticket_Booking.Passenger;
using Ticket_Booking.Presentation;
using Ticket_Booking.Repository;

namespace Ticket_Booking;

class MainClass
{
    public static void Main()
    {
        string flightsFile = "Files/Flights.csv";
        string bookingFile = "Files/Bookings.csv";
        var flightRepo = new FlightRepository(flightsFile);
        var flightService = new FlightService (flightRepo);
        var bookingRepo = new BookingRepository (bookingFile);
        var bookingService = new BookingService(flightRepo, bookingRepo);
        while (true)
        {
            MainMenuOptions choice = UserInterface.PrintMenu();
            switch (choice)
            {
                case MainMenuOptions.Passenger:
                    PassengerOptions option = PassengerInterface.PrintPassengerMenu();
                    switch (option)
                    {
                        case PassengerOptions.Search:
                            SearchFlight searchFlight = new(flightService);
                            searchFlight.Search();
                            break;
                        case PassengerOptions.AddBooking:
                            AddBookingUI bookingUI = new(flightService, bookingService);
                            bookingUI.BookFlight();
                            break;
                        default:
                            Console.WriteLine("Invalid passenger option.");
                            break;
                    }
                    break;
                case MainMenuOptions.Manager:
                    ManagerOptions ManagerOption = ManagerInterface.PrintPassengerMenu();
                    switch (ManagerOption)
                    {
                        case ManagerOptions.UploadUpdate:
                            Console.Write("Enter path to the flight CSV file: ");
                            string? csvPath = Console.ReadLine();
                            if (string.IsNullOrEmpty(csvPath))
                            {
                                Console.WriteLine("Invalid Name");
                                continue;
                            }
                            var errors = flightService.BatchUploadFlights(csvPath);
                            if (errors.Count != 0)
                            {
                                Console.WriteLine("Validation Errors:");
                                foreach (var err in errors)
                                    Console.WriteLine(err);
                            }
                            break;
                        case ManagerOptions.ValidationInfo:
                            var validationInfo=flightService.ValidationInfo();
                            foreach (var rule in validationInfo)
                            {
                                Console.WriteLine($"Field: {rule.Field}, Type: {rule.Type}, Constraints: {rule.Constraints}");
                            }
                            break;
                    }
                    break;
            }
        }
    }
}