using System;
using System.Collections.Generic;
using System.Text;

namespace APD_DAP_LoadTest.Models
{

    public class Req09Body
    {
        public Header header { get; set; }
        public Parameter Parameter { get; set; }
    }

    public class Header
    {
        public string fno { get; set; }
        public string customer { get; set; }
        public string customerpassword { get; set; }
        public string scheduledepartureUTC { get; set; }
        public string requestid { get; set; }
        public string arrivalairportICAO { get; set; }
        public string departureairportICAO { get; set; }
        public string uniqid { get; set; }
        public string acregid { get; set; }
    }

    public class Parameter
    {
        public string Rev { get; set; }
        public int ReleaseWt { get; set; }
        public string PrimaryRWY { get; set; }
        public int LandFuel { get; set; }
        public int ReqFuel { get; set; }
        public int TaxiFuel { get; set; }
    }

}
