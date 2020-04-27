using System;
using System.Collections.Generic;
using System.Text;

namespace APD_DAP_LoadTest.Models
{
    public class Req_06Body
    {
        public Flightkey FlightKey { get; set; }
        public string TargetAirportCode { get; set; }
        public string AcarsMessageFormat { get; set; }
        public string TakeoffOrLanding { get; set; }
    }
    

}
