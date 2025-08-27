using Moq;
using Xunit;
using TicketBooking.Repository;
using TicketBooking.Models;
using TicketBooking.Service;

namespace TicketBooking.TicketBooking.Tests;

public class CancelBookingTests
{
    private readonly Mock<IFlightRepository> _flightRepoMock;
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly BookingService _bookingService;

    public CancelBookingTests()
    {
        _flightRepoMock = new Mock<IFlightRepository>();
        _bookingRepoMock = new Mock<IBookingRepository>();
        _bookingService = new BookingService(_flightRepoMock.Object, _bookingRepoMock.Object);
    }

    [Fact]
    public void CancelBooking_ValidBookingId_DeletesBooking()
    {
        // Arrange
        var bookingId = "123";

        // Simulate that GetById returns a booking
        _bookingRepoMock.Setup(r => r.GetById(bookingId))
                .Returns(new Booking { BookingId = bookingId,
                    PassengerId ="1",
                    FlightId = "F001",
                    Class = FlightClass.Economy,
                    Price = 1});

        // Act
        _bookingService.CancelBooking(bookingId);

        // Assert
        _bookingRepoMock.Verify(r => r.DeleteOne(bookingId), Times.Once);
    }

    [Fact]
    public void CancelBooking_InvalidBookingId_DoesNotDeleteBooking()
    {
        // Arrange
        var invalidId = "999";

        // Simulate GetById returns null
        _bookingRepoMock.Setup(r => r.GetById(invalidId)).Returns((Booking?)null);

        // Act
        _bookingService.CancelBooking(invalidId);

        // Assert: Verify DeleteOne is never called
        _bookingRepoMock.Verify(r => r.DeleteOne(It.IsAny<string>()), Times.Never);
    }
}