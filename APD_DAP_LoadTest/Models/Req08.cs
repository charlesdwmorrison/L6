using System;
using System.Collections.Generic;
using System.Text;

namespace APD_DAP_LoadTest.Models
{
    public class Req08Body
    {
        public string TestCaseNumber { get; set; }
        public Flightkey FlightKey { get; set; }
        public string TargetAirportCode { get; set; }
        public string[] RunwayDesignatorList { get; set; }
        public string OAT { get; set; }
        public string WindSpeed { get; set; }
        public string WindGustSpeed { get; set; }
        public string WindDirection { get; set; }
        public string WindComponent { get; set; }
        public string DirectionType { get; set; }
        public int TailwindLimit { get; set; }
        public int HeadwindLimit { get; set; }
        public string QNH { get; set; }
        public string TakeoffWeight { get; set; }
        public string Flap { get; set; }
        public string CalculationMethod { get; set; }
        public string Thrust { get; set; }
        public string Bleed { get; set; }
        public string AntiIce { get; set; }
        public string AcarsRunwayCondition { get; set; }
        public string Contamination { get; set; }
        public string ContaminationDepth { get; set; }
        public object[] MelCdls { get; set; }
        public bool ReturnAcarsPayloadOnly { get; set; }
    }


}
