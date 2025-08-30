using Moq;
using Xunit;
using System.Collections.Generic;
using TicketBooking.Repository;
using TicketBooking.Models;
using TicketBooking.Service;

namespace TicketBooking.TicketBooking.Tests;
public class UpdateBookingTests
{
    private readonly Mock<IFlightRepository> _flightRepoMock;
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly BookingService _bookingService;

    public UpdateBookingTests()
    {
        _flightRepoMock = new Mock<IFlightRepository>();
        _bookingRepoMock = new Mock<IBookingRepository>();
        _bookingService = new BookingService(_flightRepoMock.Object, _bookingRepoMock.Object);
    }

    [Fact]
    public void UpdateOne_ValidBookingAndFlight_UpdatesBookingAndFlight()
    {
        // Arrange
        var bookingId = "NewFlight";
        var flightId = "1";
        var flightClass = FlightClass.Business;

        var booking = new Booking
        {
            BookingId = bookingId,
            PassengerId ="1",
            FlightId = "OldFlight",
            Class = FlightClass.Economy,
            Price = 100.0
        };

        var flight = new Flight
        {
            Id = flightId,
            DepartureCountry = "USA",
            DestinationCountry = "UK",
            DepartureDate = new DateTime(2025, 1, 1),
            DepartureAirport = "JFK",
            ArrivalAirport = "LHR",
            Prices = new Dictionary<FlightClass, double>
                {
                    { FlightClass.Economy, 500.0 },
                    { FlightClass.Business, 1200.0 }
                },
            AvailableSeats = new Dictionary<FlightClass, int>
                {
                    { FlightClass.Business, 500 }
                }
        };

        _bookingRepoMock.Setup(r => r.GetById(bookingId)).Returns(booking);
        _flightRepoMock.Setup(r => r.GetFlightById(flightId)).Returns(flight);

        // Act
        _bookingService.UpdateOne(bookingId, flightId, flightClass);

        // Assert
        Assert.Equal(flightId, booking.FlightId);
        Assert.Equal(flightClass, booking.Class);
        Assert.Equal(1200, booking.Price);
        Assert.Equal(499, flight.AvailableSeats[flightClass]);

        _bookingRepoMock.Verify(r => r.UpdateOne(booking), Times.Once);
        _flightRepoMock.Verify(r => r.UpdateFlight(flight), Times.Once);
    }

    [Fact]
    public void UpdateOne_InvalidBookingId_DoesNotUpdateAnything()
    {
        // Arrange
        _bookingRepoMock.Setup(r => r.GetById("invalid")).Returns((Booking?)null);

        // Act
        _bookingService.UpdateOne("invalid", "F1", FlightClass.Economy);

        // Assert
        _bookingRepoMock.Verify(r => r.UpdateOne(It.IsAny<Booking>()), Times.Never);
        _flightRepoMock.Verify(r => r.UpdateFlight(It.IsAny<Flight>()), Times.Never);
    }

    [Fact]
    public void UpdateOne_InvalidFlightId_DoesNotUpdateBooking()
    {
        // Arrange
        var bookingId = "B123";
        var booking = new Booking
        {
            BookingId = bookingId,
            PassengerId ="1",
            FlightId = "OldFlight",
            Class = FlightClass.Economy,
            Price = 100.0
        };
        _bookingRepoMock.Setup(r => r.GetById(bookingId)).Returns(booking);
        _flightRepoMock.Setup(r => r.GetFlightById("invalid")).Returns((Flight?)null);

        // Act
        _bookingService.UpdateOne(bookingId, "invalid", FlightClass.Economy);

        // Assert
        _bookingRepoMock.Verify(r => r.UpdateOne(It.IsAny<Booking>()), Times.Never);
        _flightRepoMock.Verify(r => r.UpdateFlight(It.IsAny<Flight>()), Times.Never);
    }

    [Fact]
    public void UpdateOne_NoAvailableSeats_DoesNotUpdateBooking()
    {
        // Arrange
        var bookingId = "B123";
        var flightId = "F456";
        var flightClass = FlightClass.FirstClass;

        var _bookingRepo = new Mock<IBookingRepository>();
        var _flightRepoMock = new Mock<IFlightRepository>();

        var booking = new Booking
        {
            BookingId = bookingId,
            PassengerId ="1",
            FlightId = "OldFlight",
            Class = FlightClass.Economy,
            Price = 100.0
        };
        var flight = new Flight
        {
            Id = flightId,
            AvailableSeats = new Dictionary<FlightClass, int> { { flightClass, 0 } },
            Prices = new Dictionary<FlightClass, double> { { flightClass, 1000.0 } },
            DepartureCountry = "USA",
            DestinationCountry = "UK",
            DepartureDate = new DateTime(2025, 1, 1),
            DepartureAirport = "JFK",
            ArrivalAirport = "LHR",
        };

        _bookingRepoMock.Setup(r => r.GetById(bookingId)).Returns(booking);
        _flightRepoMock.Setup(r => r.GetFlightById(flightId)).Returns(flight);

        // Act
        _bookingService.UpdateOne(bookingId, flightId, flightClass);

        // Assert
        _bookingRepoMock.Verify(r => r.UpdateOne(It.IsAny<Booking>()), Times.Never);
        _flightRepoMock.Verify(r => r.UpdateFlight(It.IsAny<Flight>()), Times.Never);
    }
}