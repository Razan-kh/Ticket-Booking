using TicketBooking.Presentation;
using TicketBooking.Repository;
using TicketBooking.Service;

namespace TicketBooking;

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
        FilterBookingsUI filterBookings = new FilterBookingsUI(bookingService);
        PassengerInterface passengerInterface = new(bookingService);
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

                        case PassengerOptions.UpdateBooking:
                            passengerInterface.UpdateBooking();
                            break;

                        case PassengerOptions.PersonalBookings:
                            PassengerInterface.PersonalBookings(bookingService);
                            break;

                        case PassengerOptions.DeleteBooking:
                            Console.Write("Enter Booking ID to cancel: ");
                            var cancelId = Console.ReadLine();
                            bookingService.CancelBooking(cancelId!);
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

                        case ManagerOptions.UploadUpdate:
                            ManagerInterface.UploadFile(flightService);
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