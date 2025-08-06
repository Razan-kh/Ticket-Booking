using Ticket_Booking.Passenger;
using Ticket_Booking.Presentation;
using Ticket_Booking.Repository;

namespace Ticket_Booking;

class MainClass
{
    public static void Main()
    {
        string flightsFile = "Files/Flights.csv";
        var flightRepo = new FlightRepository(flightsFile);
        var flightService = new FlightService { Repository = flightRepo };
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