using System;
using System.Collections.Generic;
using System.Text;

namespace APD_DAP_LoadTest.Models
{

    public class Req07Body
    {
        public string CalculationMethod { get; set; }
        public Flightkey FlightKey { get; set; }
        public string TargetAirportCode { get; set; }
        public string MessageFormat { get; set; }
        public string ScheduledDepartureDay { get; set; }
        public string[] RunwayDesignatorList { get; set; }
        public string LandingWeight { get; set; }
        public string OAT { get; set; }
        public string QNH { get; set; }
        public string WindDirection { get; set; }
        public string WindSpeed { get; set; }
        public string VectorType { get; set; }
        public string WindGustSpeed { get; set; }
        public int TailwindLimit { get; set; }
        public int HeadwindLimit { get; set; }
        public string Flaps { get; set; }
        public string Contamination { get; set; }
        public string MuNumber { get; set; }
        public string Autobrake { get; set; }
        public string VrefIce { get; set; }
        public string ApproachCategory { get; set; }
        public string Bleed { get; set; }
        public string AntiIce { get; set; }
        public string IceInflight { get; set; }
        public string ThrustReversers { get; set; }
        public string AntiSkid { get; set; }
        public string TailSkidMEL { get; set; }
        public string NonNormals { get; set; }
        public object[] MelCdls { get; set; }
    }

}
