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
                        case PassengerOptions.PersonalBookings:
                            Console.WriteLine("Enter Your Id");
                            var passengerId = Console.ReadLine();
                            if(string.IsNullOrEmpty(passengerId))
                                Console.WriteLine("Please enter a valid Id");
                            var bookings = bookingService.GetBookingsForPassenger(passengerId!);
                            foreach (var b in bookings)
                                Console.WriteLine(b);
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