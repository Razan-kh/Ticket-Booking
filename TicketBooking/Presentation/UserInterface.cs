namespace TicketBooking.Presentation;

class UserInterface
{
    public static MainMenuOptions PrintMenu()
    {
        while (true)
        {
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("- If you are a passenger, Press 1");
            Console.WriteLine("- If you are  a manager, Press 2");
            string? consoleChoice = Console.ReadLine();
            if (!int.TryParse(consoleChoice, out int numericChoice))
            {
                Console.WriteLine("Invalid option. Please try again.");
                continue;
            }
            MainMenuOptions choice = (MainMenuOptions)numericChoice;
            return choice;
        }
    }
}