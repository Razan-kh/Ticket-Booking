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
        var flightRepo = new FlightRepository { FilePath = flightsFile };
        var flightService = new FlightService { Repository = flightRepo };
        var bookingRepo = new BookingRepository { FilePath = bookingFile };
        var bookingService = new BookingService { FlightRepo = flightRepo, BookingRepo = bookingRepo };
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
                            FlightUI ui = new (flightService);
                            ui.Run();
                            break;
                        case PassengerOptions.AddBooking:
                            BookingUI bookingUI = new BookingUI(flightService, bookingService);
                            bookingUI.Run();
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