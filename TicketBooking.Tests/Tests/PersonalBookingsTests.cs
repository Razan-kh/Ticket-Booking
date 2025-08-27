using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using TicketBooking.Repository;
using TicketBooking.Models;
using TicketBooking.Service;

namespace TicketBooking.TicketBooking.Tests;
public class PersonalBookingsTests
{
    private readonly Mock<IFlightRepository> _flightRepoMock;
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly BookingService _bookingService;

    public PersonalBookingsTests()
    {
        _flightRepoMock = new Mock<IFlightRepository>();
        _bookingRepoMock = new Mock<IBookingRepository>();
        _bookingService = new BookingService(_flightRepoMock.Object, _bookingRepoMock.Object);
    }

    [Fact]
    public void GetBookingsForPassenger_ReturnsOnlyPassengerBookings()
    {
        // Arrange
        var passengerId = "P123";
        var bookings = new List<Booking>
        {
            new Booking { BookingId = "B1", PassengerId = "P123", FlightId = "F1", Class = FlightClass.Economy, Price = 100.0 },
            new Booking { BookingId = "B2", PassengerId = "P123", FlightId = "F2", Class = FlightClass.Business, Price = 200.0 },
            new Booking { BookingId = "B3", PassengerId = "P999", FlightId = "F3", Class = FlightClass.Economy, Price = 300.0 }
        };

        _bookingRepoMock.Setup(r => r.GetByPassengerId(passengerId))
                .Returns(bookings.Where(b => b.PassengerId == passengerId).ToList());

        // Act
        var result = _bookingService.GetBookingsForPassenger(passengerId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, b => Assert.Equal(passengerId, b.PassengerId));
    }

    [Fact]
    public void GetBookingsForPassenger_NoBookings_ReturnsEmptyList()
    {
        // Arrange
        var passengerId = "P123";
        _bookingRepoMock.Setup(r => r.GetByPassengerId(passengerId)).Returns(new List<Booking>());

        // Act
        var result = _bookingService.GetBookingsForPassenger(passengerId);

        // Assert
        Assert.Empty(result);
    }
}