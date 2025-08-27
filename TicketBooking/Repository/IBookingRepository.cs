using TicketBooking.Models;

namespace TicketBooking.Repository;

public interface IBookingRepository
{
    public void SaveBooking(Booking booking);
    public List<Booking> FilterBookings(BookingsFilter bookingFilter, List<Flight> flights);
    public Booking? GetById(string bookingId);
    public void UpdateOne(Booking updated);
    public List<Booking> GetByPassengerId(string passengerId);
    public void DeleteOne(string bookingId);
}