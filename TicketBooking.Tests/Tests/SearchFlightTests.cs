using Xunit;
using Moq;
using FluentAssertions;
using System;
using System.Collections.Generic;
using TicketBooking.Models;
using TicketBooking.Repository;
using TicketBooking.Service;

namespace TicketBooking.TicketBooking.Tests;

public class SearchFlightTests
{
    private readonly Mock<IFlightRepository> _mockFlightRepo;
    private readonly FlightService _flightService;

    public SearchFlightTests()
    {
        _mockFlightRepo = new Mock<IFlightRepository>();
        _flightService = new FlightService(_mockFlightRepo.Object);
    }

    [Fact]
    public void SearchFlights_ShouldReturnMatchingFlight_WhenFilterMatches()
    {
        // Arrange
        var flights = new List<Flight>
        {
            new Flight
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
            }
        };

        var filter = new FlightFilter
        {
            DepartureCountry = "USA",
            DestinationCountry = "UK",
            DepartureDate = new DateTime(2025, 1, 1),
            ClassType = FlightClass.Economy,
            MaxPrice = 600
        };

        _mockFlightRepo.Setup(r => r.SearchFlights(It.IsAny<FlightFilter>()))
                .Returns<FlightFilter>(filter =>
                    flights.FindAll(f =>
                        (string.IsNullOrEmpty(filter.DepartureCountry) || f.DepartureCountry == filter.DepartureCountry) &&
                        (string.IsNullOrEmpty(filter.DestinationCountry) || f.DestinationCountry == filter.DestinationCountry) &&
                        (!filter.DepartureDate.HasValue || f.DepartureDate.Date == filter.DepartureDate.Value.Date) &&
                        (!filter.MaxPrice.HasValue ||
                            (filter.ClassType.HasValue &&
                             f.Prices.ContainsKey(filter.ClassType.Value) &&
                             f.Prices[filter.ClassType.Value] <= filter.MaxPrice.Value))
                    )
                );
                
        // Act
        var result = _flightService.SearchFlights(filter);
        var flight = result.First();

        // Assert 
        Assert.Single(result);                 
        Assert.Equal("F1", flight.Id);          
    }

    [Fact]
    public void SearchFlights_ShouldReturnEmpty_WhenNoMatch()
    {
        // Arrange
        var flights = new List<Flight>
        {
            new Flight
            {
                Id = "F1",
                DepartureCountry = "USA",
                DestinationCountry = "UK",
                DepartureDate = new DateTime(2025, 1, 1),
                DepartureAirport = "JFK",
                ArrivalAirport = "LHR",
                Prices = new Dictionary<FlightClass, double>
                {
                    { FlightClass.Economy, 500 }
                },
                AvailableSeats = new Dictionary<FlightClass, int>
                {
                    { FlightClass.Economy, 500 }
                }
            }
        };

        _mockFlightRepo.Setup(r => r.SearchFlights(It.IsAny<FlightFilter>()))
                .Returns(new List<Flight>());

        var filter = new FlightFilter
        {
            DepartureCountry = "France"
        };

        // Act
        var result = _flightService.SearchFlights(filter);

        // Assert
        Assert.Empty(result);
    }
}