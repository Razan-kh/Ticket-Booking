using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using TicketBooking.Repository;
using TicketBooking.Models;
using TicketBooking.Service;

public class BookingFilterTests
{
    private readonly Mock<IBookingRepository> _bookingRepoMock;
    private readonly Mock<IFlightRepository> _flightRepoMock;
    private readonly BookingService _service;
    private readonly List<Booking> _bookings;
    private readonly List<Flight> _flights;

    public BookingFilterTests()
    {
        _bookingRepoMock = new Mock<IBookingRepository>();
        _flightRepoMock = new Mock<IFlightRepository>();

        _bookings = CreateSampleBookings();
        _flights = CreateSampleFlights();

        SetupFlightRepoMock();
        SetupBookingRepoMock();

        _service = new BookingService(_flightRepoMock.Object, _bookingRepoMock.Object);
    }

    [Fact]
    public void FilterByFlightId_ReturnsCorrectBooking()
    {
        var filter = new BookingsFilter { FlightId = "F1" };
        var result = _service.FilterBookings(filter);

        Assert.Single(result);
        Assert.Equal("B1", result.First().BookingId);
    }

    [Fact]
    public void FilterByPassengerId_ReturnsOnlyPassengerBookings()
    {
        var filter = new BookingsFilter { PassengerId = "P1" };

        var result = _service.FilterBookings(filter);

        Assert.Equal(2, result.Count);
        Assert.All(result, b => Assert.Equal("P1", b.PassengerId));
    }

    [Fact]
    public void FilterByPrice_ReturnsMatchingBooking()
    {
        var filter = new BookingsFilter { Price = 200 };

        var result = _service.FilterBookings(filter);

        Assert.Single(result);
        Assert.Equal("B2", result.First().BookingId);
    }

    [Fact]
    public void FilterByDepartureCountry_ReturnsMatchingBookings()
    {
        var filter = new BookingsFilter { DepartureCountry = "USA" };

        var result = _service.FilterBookings(filter);

        Assert.Equal(2, result.Count);
        Assert.All(result, b => Assert.Contains(b.BookingId, new[] { "B1", "B2" }));
    }

    [Fact]
    public void FilterByMultipleCriteria_ReturnsCorrectBooking()
    {
        var filter = new BookingsFilter
        {
            DepartureCountry = "USA",
            DestinationCountry = "France",
            FlightClass = FlightClass.Business
        };

        var result = _service.FilterBookings(filter);

        Assert.Single(result);
        Assert.Equal("B2", result.First().BookingId);
        }
    private List<Booking> CreateSampleBookings() => new()
        {
            new Booking { BookingId = "B1", PassengerId = "P1", FlightId = "F1", Class = FlightClass.Economy, Price = 100 },
            new Booking { BookingId = "B2", PassengerId = "P1", FlightId = "F2", Class = FlightClass.Business, Price = 200 },
            new Booking { BookingId = "B3", PassengerId = "P2", FlightId = "F3", Class = FlightClass.FirstClass, Price = 300 }
        };

    private List<Flight> CreateSampleFlights() => new()
    {
        new Flight {
            Id = "F1",
            DepartureCountry = "USA",
            DestinationCountry = "UK",
            DepartureDate = new DateTime(2025,1,1),
            DepartureAirport = "JFK",
            ArrivalAirport = "LHR",
            Prices = new Dictionary<FlightClass, double> { { FlightClass.Economy, 500.0 } },
            AvailableSeats = new Dictionary<FlightClass, int> { { FlightClass.Business, 500 } }
        },
        new Flight {
            Id = "F2",
            DepartureCountry = "USA",
            DestinationCountry = "France",
            DepartureDate = new DateTime(2025,1,2),
            DepartureAirport = "JFK",
            ArrivalAirport = "CDG",
            Prices = new Dictionary<FlightClass, double> { { FlightClass.Economy, 500.0 } },
            AvailableSeats = new Dictionary<FlightClass, int> { { FlightClass.Business, 500 } }
        },
        new Flight {
            Id = "F3",
            DepartureCountry = "Canada",
            DestinationCountry = "Germany",
            DepartureDate = new DateTime(2025,1,3),
            DepartureAirport = "YYZ",
            ArrivalAirport = "FRA",
            Prices = new Dictionary<FlightClass, double> { { FlightClass.FirstClass, 1200.0 } },
            AvailableSeats = new Dictionary<FlightClass, int> { { FlightClass.FirstClass, 100 } }
        }
    };

    private void SetupFlightRepoMock()
    {
        _flightRepoMock.Setup(r => r.GetAllFlights()).Returns(_flights);
    }

    private void SetupBookingRepoMock()
    {
        _bookingRepoMock.Setup(r => r.FilterBookings(It.IsAny<BookingsFilter>(), It.IsAny<List<Flight>>()))
            .Returns((BookingsFilter filter, List<Flight> flights) =>
                _bookings
                    .Where(b => filter.PassengerId == null || b.PassengerId == filter.PassengerId)
                    .Where(b => filter.FlightId == null || b.FlightId == filter.FlightId)
                    .Where(b => filter.Price == null || b.Price == filter.Price)
                    .Where(b => filter.DepartureCountry == null || flights.First(f => f.Id == b.FlightId).DepartureCountry == filter.DepartureCountry)
                    .Where(b => filter.DestinationCountry == null || flights.First(f => f.Id == b.FlightId).DestinationCountry == filter.DestinationCountry)
                    .Where(b => filter.FlightClass == null || b.Class == filter.FlightClass)
                    .ToList());
    }
}
