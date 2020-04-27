using System;
using System.Collections.Generic;
using System.Text;

namespace APD_DAP_LoadTest.Models
{
    public class Req02Body
    {
        public string AcarsAeroDataRequestMessage { get; set; }
        public string CalculationMethod { get; set; }
        public Flightkey FlightKey { get; set; }
        public string TargetAirportCode { get; set; }
        public string MessageFormat { get; set; }
        public string ScheduledDepartureDay { get; set; }
        public string[] RunwayDesignatorList { get; set; }
        public string LandingWeight { get; set; }
        public int OAT { get; set; }
        public string QNH { get; set; }
        public string WindDirection { get; set; }
        public string WindSpeed { get; set; }
        public string VectorType { get; set; }
        public string WindGustSpeed { get; set; }
        public string TailwindLimit { get; set; }
        public string HeadwindLimit { get; set; }
        public string TestCaseNumber { get; set; }
        public string Flaps { get; set; }
        public string Contamination { get; set; }
        public string ContaminationDepth { get; set; }
        public string BrakingAction { get; set; }
        public string MuNumber { get; set; }
        public string Autobrake { get; set; }
        public string VrefIce { get; set; }
        public string ApproachCategory { get; set; }
        public string Bleeds { get; set; }
        public string AntiIce { get; set; }
        public string IceInflight { get; set; }
        public string AntiSkid { get; set; }
        public string TailSkidMEL { get; set; }
        public string NonNormals { get; set; }
        public object[] MELCDLs { get; set; }
    }

}
