using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using TicketBooking.Repository;
using TicketBooking.Models;
using TicketBooking.Service;

public class FlightValidationTests
{
    private readonly Mock<IFlightRepository> _flightRepoMock;
    private readonly FlightService _flightService;

    public FlightValidationTests()
    {
        _flightRepoMock = new Mock<IFlightRepository>();
        _flightService = new FlightService(_flightRepoMock.Object);
    }

    [Fact]
    public void ValidationInfo_ReturnsFlightValidationRules()
    {
        // Arrange
        var mockRules = new List<(string Field, string Type, string Constraints)>
        {
            ("DepartureCountry", "String", "Required"),
            ("DepartureDate", "DateTime", "Required, Date"),
            ("ArrivalCountry", "String", ""),
        };

        _flightRepoMock.Setup(r => r.GetFlightValidationRules())
                       .Returns(mockRules);

        // Act
        var result = _flightService.ValidationInfo();

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Contains(result, r => r.Field == "DepartureCountry" && r.Type == "String" && r.Constraints.Contains("Required"));
        Assert.Contains(result, r => r.Field == "DepartureDate" && r.Type == "DateTime" && r.Constraints.Contains("Date"));
        Assert.Contains(result, r => r.Field == "ArrivalCountry" && r.Type == "String");
    }

    [Fact]
    public void ValidationInfo_EmptyRules_ReturnsEmptyList()
    {
        // Arrange
        _flightRepoMock.Setup(r => r.GetFlightValidationRules())
                       .Returns(new List<(string Field, string Type, string Constraints)>());

        // Act
        var result = _flightService.ValidationInfo();

        // Assert
        Assert.Empty(result);
    }
}