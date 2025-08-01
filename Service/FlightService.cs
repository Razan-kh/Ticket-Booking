namespace Ticket_Booking.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using Ticket_Booking.Models;

public class FlightService
{
    private FlightRepository _repository;

    public FlightService(FlightRepository repository)
    {
        _repository = repository;
    }

    public List<Flight> SearchFlights(string? depCountry, string? destCountry, DateTime? date, string? depAirport, string? arrAirport, double? maxPrice, FlightClass? selectedClass)
    {
        var flights = _repository.GetAllFlights();
        return flights.Where(f =>
            (string.IsNullOrEmpty(depCountry) || f.DepartureCountry.Equals(depCountry, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(destCountry) || f.DestinationCountry.Equals(destCountry, StringComparison.OrdinalIgnoreCase)) &&
            (!date.HasValue || f.DepartureDate.Date == date.Value.Date) &&
            (string.IsNullOrEmpty(depAirport) || f.DepartureAirport.Equals(depAirport, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(arrAirport) || f.ArrivalAirport.Equals(arrAirport, StringComparison.OrdinalIgnoreCase)) &&
            (!maxPrice.HasValue || (selectedClass.HasValue && f.Prices[selectedClass.Value] <= maxPrice.Value))
        ).ToList();
    }
}
