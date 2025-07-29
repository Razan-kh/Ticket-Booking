namespace Ticket_Booking.Repository;
// BookingRepository.cs
public class BookingRepository
{
    private List<Booking> _bookings = new();

    public void SaveBooking(Booking booking)
    {
        _bookings.Add(booking);
        SaveToCsv(booking);
    }

    private void SaveToCsv(Booking booking)
    {
        // Append to Booking.csv
        var line = $"{booking.BookingId},{booking.PassengerId},{booking.FlightId},{booking.Class},{booking.Price}";
        File.AppendAllText("Bookings.csv", line + Environment.NewLine);
    }
}
