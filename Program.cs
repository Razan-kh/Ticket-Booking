using Ticket_Booking.Presentation;

namespace Ticket_Booking;

class MainClass
{
    public static void Main()
    {
        {
            while (true)
            {
                MainMenuOptions choice = UserInterface.PrintMenu();
                switch (choice)
                {
                    case MainMenuOptions.Passenger:
                        break;
                    case MainMenuOptions.Manager:
                        break;
                }
            }
        }
    }
}