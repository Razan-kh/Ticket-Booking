using Ticket_Booking.Passenger;
using Ticket_Booking.Presentation;
using Ticket_Booking.Repository;

namespace Ticket_Booking;

class MainClass
{
    public static void Main()
    {   
        var flightRepo = new FlightRepository();
        var flightService = new FlightService(flightRepo);
        var bookingRepo = new BookingRepository();
        var bookingService = new BookingService(flightRepo,bookingRepo);
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
                            FlightUI ui = new FlightUI(flightService);
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