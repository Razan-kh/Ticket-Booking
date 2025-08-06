using Ticket_Booking.Models;

namespace Ticket_Booking.Repository;

public class BookingRepository
{
    private List<Booking> _bookings = [];
    private readonly string _filePath;
    
    public BookingRepository(string filePath)
    {
        _filePath = filePath;
        _bookings = LoadBookings(_filePath);
    }
    public void SaveBooking(Booking booking)
    {
        booking.BookingId = _bookings.Count.ToString();
        _bookings.Add(booking);
        SaveToCsv(booking);
    }

    private void SaveToCsv(Booking booking)
    {
        var line = $"{booking.BookingId},{booking.PassengerId},{booking.FlightId},{booking.Class},{booking.Price}";
        File.AppendAllText(_filePath, line + Environment.NewLine);
    }

    public static List<Booking> LoadBookings(string filePath)
    {
        var bookings = new List<Booking>();

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"The file at path '{filePath}' was not found.");
        var lines = File.ReadAllLines(filePath);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var parts = line.Split(',');

            if (parts.Length < 5)
                throw new FormatException($"Invalid booking record: {line}");

            if (!Enum.TryParse(parts[3], out FlightClass classType))
                throw new FormatException($"Invalid class type in booking: {parts[3]}");

            if (!double.TryParse(parts[4], out var price))
                throw new FormatException($"Invalid price in booking: {parts[4]}");

            var booking = new Booking
            {
                BookingId = parts[0],
                PassengerId = parts[1],
                FlightId = parts[2],
                Class = classType,
                Price = price
            };

            bookings.Add(booking);
        }
        return bookings;
    }
}
