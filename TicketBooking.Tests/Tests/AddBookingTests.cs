using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using TicketBooking.Repository;
using TicketBooking.Models;
using TicketBooking.Service;

namespace TicketBooking.TicketBooking.Tests;

public class AddBookingTests
{
    private readonly Mock<IFlightRepository> _flightRepoMock;
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly BookingService _bookingService;

    public AddBookingTests()
    {
        _flightRepoMock = new Mock<IFlightRepository>();
        _bookingRepoMock = new Mock<IBookingRepository>();
        _bookingService = new BookingService(_flightRepoMock.Object, _bookingRepoMock.Object);
    }

    [Fact]
    public void BookFlight_ShouldSaveBooking_WhenSeatsAvailable()
    {
        // Arrange
        var flight = new Flight
        {
            Id = "F1",
            DepartureCountry = "USA",
            DestinationCountry = "UK",
            DepartureDate = new DateTime(2025, 1, 1),
            DepartureAirport = "JFK",
            ArrivalAirport = "LHR",
            Prices = new Dictionary<FlightClass, double>
                {
                    { FlightClass.Economy, 500 },
                    { FlightClass.Business, 1200 }
                },
            AvailableSeats = new Dictionary<FlightClass, int>
                {
                    { FlightClass.Economy, 500 }
                }
        };

        _flightRepoMock.Setup(r => r.GetFlightById("F1")).Returns(flight);

        // Act
        _bookingService.BookFlight("P1", "F1", FlightClass.Economy);

        // Assert
        _bookingRepoMock.Verify(r => r.SaveBooking(It.Is<Booking>(
            b => b.PassengerId == "P1" &&
                 b.FlightId == "F1" &&
                 b.Class == FlightClass.Economy &&
                 b.Price == 500
        )), Times.Once);

        Assert.Equal(499, flight.AvailableSeats[FlightClass.Economy]);
        _flightRepoMock.Verify(r => r.UpdateFlight(flight), Times.Once);
    }

    [Fact]
    public void BookFlight_ShouldNotSave_WhenNoSeatsAvailable()
    {
        // Arrange
        var flight = new Flight
            {
                Id = "F1",
                DepartureCountry = "USA",
                DestinationCountry = "UK",
                DepartureDate = new DateTime(2025, 1, 1),
                DepartureAirport = "JFK",
                ArrivalAirport = "LHR",
                Prices = new Dictionary<FlightClass, double>
                {
                    { FlightClass.Economy, 500 },
                    { FlightClass.Business, 1200 }
                },
                AvailableSeats = new Dictionary<FlightClass, int>
                {
                    { FlightClass.Economy, 0 }
                }
            };

        _flightRepoMock.Setup(r => r.GetFlightById("F1")).Returns(flight);

        // Act
        _bookingService.BookFlight("P1", "F1", FlightClass.Economy);

        // Assert
        _bookingRepoMock.Verify(r => r.SaveBooking(It.IsAny<Booking>()), Times.Never);
        _flightRepoMock.Verify(r => r.UpdateFlight(It.IsAny<Flight>()), Times.Never);
        Assert.Equal(0, flight.AvailableSeats[FlightClass.Economy]);
    }

    [Fact]
    public void BookFlight_ShouldThrowException_WhenFlightNotFound()
    {
        // Arrange
        _flightRepoMock.Setup(r => r.GetFlightById("Invalid")).Returns((Flight?)null);

        // Act + Assert
        Assert.Throws<Exception>(() => _bookingService.BookFlight("P1", "Invalid", FlightClass.Economy));

        _bookingRepoMock.Verify(r => r.SaveBooking(It.IsAny<Booking>()), Times.Never);
        _flightRepoMock.Verify(r => r.UpdateFlight(It.IsAny<Flight>()), Times.Never);
    }
}