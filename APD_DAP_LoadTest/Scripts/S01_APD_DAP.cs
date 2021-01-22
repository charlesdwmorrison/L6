//using APD_DAP_LoadTest.Models;
//using L6;
//using Newtonsoft.Json;
//using RestSharp;
//using System.Collections.Generic;
//using System.Threading;

//namespace ADP_DAP_LoadTest
//{
//    public class S01_APD_DAP
//    {

//        string urlPrefix = "http://fogs.alaskaair.com";  // "http://fogs.qa.webservice.alaskaair.com";

//        private SendRequests sr;
//        static string[] postBodyLst;

//        public S01_APD_DAP()
//        {
//            sr = new SendRequests(1000);
//            postBodyLst = BuildListOfPostBodies();
//        }

//        // correlation variables, initialized value is empty. Will fill after getting response
//        //public string empId = "";
//        // This is an Example only of how correlation would be inserted
//        public static Dictionary<string, string> empId = new Dictionary<string, string>
//        {
//            {"CorrelationVar01"," "}
//        };



//        public void Req00()
//        {
//            Req00Body req00Body = new Req00Body();
//            Req req = new Req
//            {
//                uri = urlPrefix + "/AircraftPerformanceData/1/airlines/AS/aircraft/performance/runways",
//                method = Method.POST,
//                //body = postBodyLst[0] // build using string          
//                body = JsonConvert.SerializeObject(req00Body) // build using NewtonSoft
//            };
//            sr.SendRequest(req);
//        }

//        public void Req01()
//        {
//            Req req = new Req
//            {
//                uri = urlPrefix + "/DispatchAircraftPerformance/1/api/landing?airlinecode=AS",
//                method = Method.POST,
//                body = postBodyLst[1]
//            };
//            sr.SendRequest(req);
//        }

//        public void Req02()
//        {
//            Req req = new Req
//            {
//                uri = urlPrefix + "/DispatchAircraftPerformance/1/api/landing?airlinecode=AS",
//                method = Method.POST,
//                body = postBodyLst[2]
//            };
//            sr.SendRequest(req);
//        }

//        public void Req03()
//        {
//            Req req = new Req
//            {
//                uri = urlPrefix +
//                          "/DispatchAircraftPerformance/1/api/waminitialize/?airlinecode=AS&fltsegid=8315627&torl=T&airportcode=SEA",
//                method = Method.GET,
//                body = postBodyLst[3]
//            };
//            sr.SendRequest(req);
//        }

//        public void Req04()
//        {
//            Req req = new Req
//            {
//                uri = urlPrefix +
//                          "/DispatchAircraftPerformance/1/api/waminitialize/?airlinecode=AS&fltsegid=8310928&torl=L&airportcode=SEA",
//                method = Method.GET,
//                body = postBodyLst[4]
//            };
//            sr.SendRequest(req);
//        }

//        public void Req05()
//        {
//            Req req = new Req
//            {
//                uri = urlPrefix + "/DispatchAircraftPerformance/1/api/takeoff?airlinecode=AS",
//                method = Method.POST,
//                body = postBodyLst[5]
//            };
//            sr.SendRequest(req);
//        }

//        public void Req06()
//        {
//            Req req = new Req
//            {
//                uri = urlPrefix + "/AircraftPerformanceData/1/airlines/AS/aircraft/performance/runways",
//                method = Method.POST,
//                body = postBodyLst[6]
//            };
//            sr.SendRequest(req);
//        }

//        public void Req07()
//        {
//            Req req = new Req
//            {
//                uri = urlPrefix + "/AircraftPerformanceData/1/airlines/AS/aircraft/performance/landing",
//                method = Method.POST,
//                body = postBodyLst[7]
//            };
//            sr.SendRequest(req);
//        }

//        public void Req08()
//        {
//            Req req = new Req
//            {
//                uri = urlPrefix + "/AircraftPerformanceData/1/airlines/AS/aircraft/performance/takeoff",
//                method = Method.POST,
//                body = postBodyLst[8]
//            };
//            sr.SendRequest(req);
//        }

//        public void Req09()
//        {
//            Req req = new Req
//            {
//                uri = "https://asa.jawstest.net/performance/api/release",
//                method = Method.POST,
//                body = postBodyLst[9]
//            };
//            sr.SendRequest(req);
//        }

//        public void Req10()
//        {
//            Req req = new Req
//            {
//                uri = urlPrefix + "/AircraftPerformanceData//1/airlines/AS/aircraft/performance/loadcloseout",
//                method = Method.POST,
//                body = postBodyLst[10]
//            };
//            sr.SendRequest(req);
//        }

//        public void Pacing(int pacingTimeinMs)
//        {
//            Thread.Sleep(pacingTimeinMs);
//        }



//        public Req00Body BuildReq00Body()
//        {
//            Req00Body req00Body = new Req00Body
//            {
//                FlightKey = new Flightkey
//                {
//                    FltSegId = 8310928,
//                    AirlineCode = "AS",
//                    AircraftAirlineCode = "AS",
//                    AircraftRegistration = "N546AS",
//                    FlightNumber = "1085",
//                    UTCDayOfFlight = "2020-02-1",
//                    OriginIATACode = "IAD",
//                    DestinationIATACode = "SEA",
//                    OriginICAOCode = "KIAD",
//                    DestinationICAOCode = "KSEA"

//                },
//                TargetAirportCode = "SEA",
//                AcarsMessageFormat = "areodata",
//                TakeoffOrLanding = "L"
//            };

//            return req00Body;
//        }

//        /// <summary>
//        ///  You can get these bodies from the "Fiddler to Code" plugin for Fiddler
//        /// </summary>
//        /// <returns></returns>
//        public string[] BuildListOfPostBodies()
//        {
//            string[] postBodyList = new string[11];

//            // Get these from fiddler, and flatten with line minifiyer tool. 
//            postBodyList[0] =
//            @"{""FlightKey"":{ ""FltSegId"":8310928,""AirlineCode"":""AS"",""AircraftAirlineCode"":""AS"",""AircraftRegistration"":""N546AS"",""FlightNumber"":""1085"",""UTCDayOfFlight"":""2020-02-17"",""OriginIATACode"":""IAD"",""DestinationIATACode"":""SEA"",""OriginICAOCode"":""KIAD"",""DestinationICAOCode"":""KSEA""},""TargetAirportCode"":""KSEA"",""AcarsMessageFormat"":""aerodata"",""TakeoffOrLanding"":""L""}";

//            postBodyList[1] =@"{ ""AcarsAeroDataRequestMessage"":"""",""CalculationMethod"":""dispatch"",""FlightKey"":{ ""FltSegId"":""8310928"",""AirlineCode"":""AS"",""AircraftAirlineCode"":"""",""AircraftRegistration"":""N546AS"",""FlightNumber"":""1085"",""UTCDayOfFlight"":""2020 - 02 - 17"",""OriginIATACode"":""IAD"",""DestinationIATACode"":""SEA"",""OriginICAOCode"":""KIAD"",""DestinationICAOCode"":""KSEA""},""TargetAirportCode"":"""",""MessageFormat"":"""",""ScheduledDepartureDay"":"""",""RunwayDesignatorList"":[""16L""],""LandingWeight"":""WEIGHT.OPTIMUM"",""OAT"":1,""QNH"":""30.30"",""WindDirection"":""190"",""WindSpeed"":""04"",""VectorType"":""0"",""WindGustSpeed"":"""",""TailwindLimit"":"""",""HeadwindLimit"":"""",""TestCaseNumber"":"""",""Flaps"":""LD.FLAP.30"",""Contamination"":""BA.NONE"",""ContaminationDepth"":""LD.DEPTH.NONE"",""MuNumber"":"""",""Autobrake"":"""",""VrefIce"":""LD.VREFICE.NONE"",""ApproachCategory"":"""",""Bleeds"":""BLEED.OPTIMUM"",""AntiIce"":""ANTIICE.OPTIMUM"",""IceInflight"":""LD.RESIDUALICE.NO"",""ThrustReversers"":""THRUST.OPTIMUM"",""AntiSkid"":"""",""TailSkidMEL"":"""",""NonNormals"":"""",""MELCDLs"":[""34-99C"",""21-12G""]}";

//            postBodyList[2] = @"{ ""AcarsAeroDataRequestMessage"":"""",""CalculationMethod"":""inflight"",""FlightKey"":{ ""FltSegId"":""8310928"",""AirlineCode"":""AS"",""AircraftAirlineCode"":"""",""AircraftRegistration"":""N546AS"",""FlightNumber"":""1085"",""UTCDayOfFlight"":""2020 - 02 - 17"",""OriginIATACode"":"""",""DestinationIATACode"":"""",""OriginICAOCode"":"""",""DestinationICAOCode"":""""},""TargetAirportCode"":"""",""MessageFormat"":"""",""ScheduledDepartureDay"":"""",""RunwayDesignatorList"":[""16L""],""LandingWeight"":""WEIGHT.MAXIMUM"",""OAT"":26,""QNH"":""30.04"",""WindDirection"":""180"",""WindSpeed"":""09"",""VectorType"":""0"",""WindGustSpeed"":""11"",""TailwindLimit"":"""",""HeadwindLimit"":"""",""TestCaseNumber"":"""",""Flaps"":""LD.FLAP.30"",""Contamination"":""LD.CONTAMINATION.MODERATE_RAIN"",""ContaminationDepth"":""LD.DEPTH.0TO8"",""BrakingAction"":""BA.GOOD"",""MuNumber"":"""",""Autobrake"":"""",""VrefIce"":""LD.VREFICE.NONE"",""ApproachCategory"":""LD.VISIBILITY.NORMAL"",""Bleeds"":""BLEED.OPTIMUM"",""AntiIce"":""ANTIICE.OPTIMUM"",""IceInflight"":""LD.RESIDUALICE.NO"",""AntiSkid"":"""",""TailSkidMEL"":"""",""NonNormals"":"""",""MELCDLs"":[] }";

//            postBodyList[3] = @""; //get

//            postBodyList[4] = @""; // get

//            postBodyList[5] = @"{""TestCaseNumber"":"""",""FlightKey"":{""FltSegId"":""8315627"",""AirlineCode"":""AS"",""AircraftAirlineCode"":"""",""AircraftRegistration"":""N461AS"",""FlightNumber"":""707"",""UTCDayOfFlight"":""2020-02-19"",""OriginIATACode"":""SEA"",""DestinationIATACode"":""RDU"",""OriginICAOCode"":""KSEA"",""DestinationICAOCode"":""KRDU""},""RunwayDesignatorList"":[""16L""],""TargetAirportCode"":""KSEA"",""OAT"":-3,""WindSpeed"":""05"",""WindGustSpeed"":"""",""WindDirection"":""020"",""WindComponent"":"""",""DirectionType"":""0"",""TailwindLimit"":"""",""HeadwindLimit"":"""",""QNH"":""30.19"",""TakeoffWeight"":""118.3"",""ZeroFuelWeight"":"""",""Flap"":""TO.FLAP.OPTIMUM"",""CalculationMethod"":""dispatch"",""Thrust"":""THRUST.OPTIMUM"",""Bleed"":""BLEED.OPTIMUM"",""AntiIce"":""ANTIICE.OPTIMUM"",""AcarsRunwayCondition"":""TO.CONTAMINATION.DRY"",""Contamination"":""BA.NONE"",""ContaminationDepth"":""TO.DEPTH.NONE"",""MELCDLs"":[""34-99C"",""21-12G""],""ReturnAcarsPayloadOnly"":true}";

//            postBodyList[6] = @"{""FlightKey"":{ ""FltSegId"":8310928,""AirlineCode"":""AS"",""AircraftAirlineCode"":""AS"",""AircraftRegistration"":""N546AS"",""FlightNumber"":""1085"",""UTCDayOfFlight"":""2020-02-17"",""OriginIATACode"":""IAD"",""DestinationIATACode"":""SEA"",""OriginICAOCode"":""KIAD"",""DestinationICAOCode"":""KSEA""},""TargetAirportCode"":""KSEA"",""AcarsMessageFormat"":""aerodata"",""TakeoffOrLanding"":""L""}";

//            postBodyList[7] = @"{""CalculationMethod"":""inflight"",""FlightKey"":{""FltSegId"":8310928,""AirlineCode"":""AS"",""AircraftAirlineCode"":""AS"",""AircraftRegistration"":""N546AS"",""FlightNumber"":""1085"",""UTCDayOfFlight"":""2020-02-17"",""OriginIATACode"":""IAD"",""DestinationIATACode"":""SEA"",""OriginICAOCode"":""KIAD"",""DestinationICAOCode"":""KSEA""},""TargetAirportCode"": ""KSEA"",""MessageFormat"":""C"",""ScheduledDepartureDay"":""05"",""RunwayDesignatorList"":[""16L""],""LandingWeight"":""90000"",""OAT"":""-3"",""QNH"":""30.19"",""WindDirection"":""020"",""WindSpeed"":""05"",""VectorType"":""TRUE"",""WindGustSpeed"":"""",""TailwindLimit"":0,""HeadwindLimit"":0,""Flaps"":""30"",""Contamination"":""0"",""MuNumber"":""0"",""Autobrake"":""0"",""VrefIce"":""0"",""ApproachCategory"":""0"",""Bleed"":""0"",""AntiIce"":""0"",""IceInflight"":""0"",""ThrustReversers"":""0"",""AntiSkid"":""0"",""TailSkidMEL"":""0"",""NonNormals"":""0"",""MelCdls"":[]}";

//            postBodyList[8] = @"{""TestCaseNumber"":"""",""FlightKey"":{""FltSegId"":8315627,""AirlineCode"":""AS"",""AircraftAirlineCode"":""AS"",""AircraftRegistration"":""N461AS"",""FlightNumber"":""707"",""UTCDayOfFlight"":""2020-02-19"",""OriginIATACode"":""SEA"",""DestinationIATACode"":""RDU"",""OriginICAOCode"":""KSEA"",""DestinationICAOCode"":""KRDU""},""TargetAirportCode"":""KSEA"",""RunwayDesignatorList"":[""16L""],""OAT"":""-3"",""WindSpeed"":""05"",""WindGustSpeed"":"""",""WindDirection"":""020"",""WindComponent"":""NONE"",""DirectionType"":""MAGNETIC"",""TailwindLimit"":0,""HeadwindLimit"":0,""QNH"":""30.19"",""TakeoffWeight"":""118.3"",""Flap"":"""",""CalculationMethod"":""inflight"",""Thrust"":""OPT"",""Bleed"":""NRM"",""AntiIce"":""NRM"",""AcarsRunwayCondition"":""DRY"",""Contamination"":"""",""ContaminationDepth"":"""",""MelCdls"":[],""ReturnAcarsPayloadOnly"":true}";

//            postBodyList[9] = @"{""header"":{""fno"":""TS546"",""customer"":""ASA"",""customerpassword"":""gABmw0RH0ijN4t"",""scheduledepartureUTC"":"" 2020-02-19"",""requestid"":""cc5a61c7-cf22-4e99-8faa-18fd3bae9a60"",""arrivalairportICAO"":""KIAD"",""departureairportICAO"":""KSEA"",""uniqid"":""ASA_999"",""acregid"":""N546AS""},""Parameter"":{""Rev"":""00"",""ReleaseWt"":150000,""PrimaryRWY"":""16L"",""LandFuel"":5000,""ReqFuel"":30000,""TaxiFuel"":500}}";

//            postBodyList[10] = @"{""FlightKey"":{""FltSegId"":8310928,""AirlineCode"":""AS"",""AircraftAirlineCode"":""AS"",""AircraftRegistration"":""N546AS"",""FlightNumber"":""1085"",""UTCDayOfFlight"":""2020-02-19"",""OriginIATACode"":""SEA"",""DestinationIATACode"":""IAD"",""OriginICAOCode"":""KSEA"",""DestinationICAOCode"":""KIAD""},""Revision"":""00"",""Timestamp"":""0001-01-01T00:00:00"",""ZeroFuelWeight"":107100,""ZeroFuelWeightCG"":20.4,""TakeoffWeight"":148000,""TakeoffWeightCG"":19.6,""FuelOnBoard"":33800}";


//            return postBodyList;
//        }




//    }
//}
