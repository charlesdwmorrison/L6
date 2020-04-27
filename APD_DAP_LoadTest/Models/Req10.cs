using System;
using System.Collections.Generic;
using System.Text;

namespace APD_DAP_LoadTest.Models
{

    public class Req10Body
    {
        public Flightkey FlightKey { get; set; }
        public string Revision { get; set; }
        public DateTime Timestamp { get; set; }
        public int ZeroFuelWeight { get; set; }
        public float ZeroFuelWeightCG { get; set; }
        public int TakeoffWeight { get; set; }
        public float TakeoffWeightCG { get; set; }
        public int FuelOnBoard { get; set; }
    }

    

}
