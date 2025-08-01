using Ticket_Booking.Models;

namespace Ticket_Booking.Repository;

public class BookingRepository
{
    private List<Booking> _bookings = new();
    private readonly string _filePath;

    public BookingRepository(string filePath)
    {
        _filePath = filePath;
    }
    public void SaveBooking(Booking booking)
    {
        booking.BookingId = GenerateNumericId().ToString();
        _bookings.Add(booking);
        SaveToCsv(booking);
    }

    private static void SaveToCsv(Booking booking)
    {
        var line = $"{booking.BookingId},{booking.PassengerId},{booking.FlightId},{booking.Class},{booking.Price}";
        File.AppendAllText("Files/Bookings.csv", line + Environment.NewLine);
    }
    
      private int GenerateNumericId()
    {
        int maxId = 0;

        if (File.Exists(_filePath))
        {
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length > 0 && int.TryParse(parts[0], out int id))
                {
                    if (id > maxId)
                        maxId = id;
                }
            }
        }

        return maxId + 1;
    }
}
