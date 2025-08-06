using Ticket_Booking.Passenger;
using Ticket_Booking.Presentation;
using Ticket_Booking.Repository;

namespace Ticket_Booking;

class MainClass
{
    public static void Main()
    {
        string flightsFile = "Files/Flights.csv";
        string bookingFile = "Files/Flights.csv";
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
                            FlightUI ui = new(flightService);
                            ui.Run();
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
                    break;
            }
        }

    }
}