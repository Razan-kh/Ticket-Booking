class Flight {
    public string Id;
    public string DepartureCountry;
    public string DestinationCountry;
    public DateTime DepartureDate;
    public string DepartureAirport;
    public string ArrivalAirport;
    public Dictionary<FlightClass, int> AvailableSeats;
    public Dictionary<FlightClass, double> Prices;
}