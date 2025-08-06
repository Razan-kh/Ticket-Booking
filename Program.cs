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
        var flightService = new FlightService(flightRepo);
        var bookingRepo = new BookingRepository(bookingFile);
        var bookingService = new BookingService(flightRepo,bookingRepo);
        FilterBookings filterBookings = new FilterBookings(bookingService);
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
                            SearchFlight ui = new SearchFlight{ Service = flightService };
                            ui.Search();
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
                    ManagerOptions managerOption = ManagerInterface.PrintPassengerMenu();
                    switch (managerOption)
                    {
                        case ManagerOptions.FilterBookings:
                            filterBookings.ShowFilterBookingsMenu();
                            break;
                    }
                break;
            }
        }

    }
}