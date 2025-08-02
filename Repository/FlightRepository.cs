namespace Ticket_Booking.Repository;

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System;
using Ticket_Booking.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class FlightRepository
{
    private readonly string _filePath;
    private List<Flight> _flights = [];

    public FlightRepository(string filePath)
    {
        _filePath = filePath;
    }
    public List<Flight> GetAllFlights()
    {
        var flights = new List<Flight>();

        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"The file at path '{_filePath}' was not found.");

        var lines = File.ReadAllLines(_filePath);

        foreach (var line in lines)
        {
            var parts = line.Split(',');

            if (parts.Length < 11) continue;

            if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var departureDate))
                throw new FormatException($"Invalid date format in: {parts[3]}");

            if (!int.TryParse(parts[6], out var seatsEconomy) ||
                !int.TryParse(parts[7], out var seatsBusiness) ||
                !int.TryParse(parts[8], out var seatsFirst))
                throw new FormatException("Invalid seat count.");

            if (!double.TryParse(parts[9], out var priceEconomy) ||
                !double.TryParse(parts[10], out var priceBusiness) ||
                !double.TryParse(parts[11], out var priceFirst))
                throw new FormatException("Invalid price format.");

            var flight = new Flight
            {
                Id = parts[0],
                DepartureCountry = parts[1],
                DestinationCountry = parts[2],
                DepartureDate = departureDate,
                DepartureAirport = parts[4],
                ArrivalAirport = parts[5],
                AvailableSeats = new Dictionary<FlightClass, int>
                {
                    { FlightClass.Economy, seatsEconomy },
                    { FlightClass.Business, seatsBusiness },
                    { FlightClass.FirstClass, seatsFirst }
                },
                Prices = new Dictionary<FlightClass, double>
                {
                    { FlightClass.Economy, priceEconomy },
                    { FlightClass.Business, priceBusiness },
                    { FlightClass.FirstClass, priceFirst }
                }
            };
            flights.Add(flight);
        }
        return flights;
    }
    public List<string> ReadFlightsFromCsv(string filePath)
    {
        var errors = new List<string>();
        _flights.Clear();
        if (!File.Exists(filePath))
        {
            errors.Add("File not found.");
            return errors;
        }

        var lines = File.ReadAllLines(filePath);
        int lineNum = 1;

        foreach (var line in lines)
        {
            var parts = line.Split(',');

            if (parts.Length < 12)
            {
                errors.Add($"Line {lineNum}: Not enough data.");
                lineNum++;
                continue;
            }

            if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var departureDate))
            {
                errors.Add($"Line {lineNum}: Invalid date format: {parts[3]}");
                lineNum++;
                continue;
            }

            if (!int.TryParse(parts[6], out var seatsEconomy) ||
                !int.TryParse(parts[7], out var seatsBusiness) ||
                !int.TryParse(parts[8], out var seatsFirst))
            {
                errors.Add($"Line {lineNum}: Invalid seat count.");
                lineNum++;
                continue;
            }

            if (!double.TryParse(parts[9], out var priceEconomy) ||
                !double.TryParse(parts[10], out var priceBusiness) ||
                !double.TryParse(parts[11], out var priceFirst))
            {
                errors.Add($"Line {lineNum}: Invalid price format.");
                lineNum++;
                continue;
            }

            var flight = new Flight
            {
                Id = parts[0],
                DepartureCountry = parts[1],
                DestinationCountry = parts[2],
                DepartureDate = departureDate,
                DepartureAirport = parts[4],
                ArrivalAirport = parts[5],
                AvailableSeats = new Dictionary<FlightClass, int>
                {
                    { FlightClass.Economy, seatsEconomy },
                    { FlightClass.Business, seatsBusiness },
                    { FlightClass.FirstClass, seatsFirst }
                },
                Prices = new Dictionary<FlightClass, double>
                {
                    { FlightClass.Economy, priceEconomy },
                    { FlightClass.Business, priceBusiness },
                    { FlightClass.FirstClass, priceFirst }
                }
            };

            var validation = ValidateFlight(flight);
            if (validation.Any())
                errors.Add($"Line {lineNum}: {string.Join(", ", validation)}");
            else
                _flights.Add(flight);

            lineNum++;
        }

        return errors;
    }

    public static List<string?> ValidateFlight(Flight flight)
    {
        var context = new ValidationContext(flight);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(flight, context, results, true);

        return results.Select(r => r.ErrorMessage).ToList();
    }
    public List<(string Field, string Type, string Constraints)> GetFlightValidationRules()
    {
        var rules = new List<(string Field, string Type, string Constraints)>();
        var properties = typeof(Flight).GetProperties();

        foreach (var prop in properties)
        {
            string fieldName = prop.Name;
            string fieldType = prop.PropertyType.Name;
            var attributes = prop.GetCustomAttributes<ValidationAttribute>();

            string constraints = string.Join(", ", attributes.Select(attr =>
            {
                if (attr is RequiredAttribute) return "Required";
                if (attr is RangeAttribute rangeAttr) return $"Range({rangeAttr.Minimum} to {rangeAttr.Maximum})";
                if (attr is DataTypeAttribute dataTypeAttr) return dataTypeAttr.DataType.ToString();
                return attr.GetType().Name.Replace("Attribute", "");
            }));

            rules.Add((fieldName, fieldType, constraints));
        }

        return rules;
    }
}
