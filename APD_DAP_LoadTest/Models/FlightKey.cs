using System;
using System.Collections.Generic;
using System.Text;

namespace APD_DAP_LoadTest.Models
{
    public class Flightkey
    {
        public int FltSegId { get; set; }
        public string AirlineCode { get; set; }
        public string AircraftAirlineCode { get; set; }
        public string AircraftRegistration { get; set; }
        public string FlightNumber { get; set; }
        public string UTCDayOfFlight { get; set; }
        public string OriginIATACode { get; set; }
        public string DestinationIATACode { get; set; }
        public string OriginICAOCode { get; set; }
        public string DestinationICAOCode { get; set; }
    }
}
