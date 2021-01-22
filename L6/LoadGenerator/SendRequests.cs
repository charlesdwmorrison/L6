using L6.LoadGenerator;
using L6.Models;
using RestSharp;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace L6
{
    public class SendRequests
    {

        public static int numRestClients = 0;
        public int responseIdForCurrentClient = 0;  // not static. Count is unique per client instance.
        public int _clientId;
        public static int responseIdForConcurrentDict = -1;
        public static int responseIdForLog = 0;
        private static DateTime testStartTime;

        RestClient client;
        LogWriter writer;
        private int thinkTimeBetweenRequests; // this IS think time between requests, not between script iterations 

        /// <summary>
        /// Constructor ensures that a new client is created for every user.
        /// </summary>
        public SendRequests(int clientId, int _thinkTimeBetweenRequests)
        {
            Interlocked.Increment(ref numRestClients);
            client = new RestClient();
            writer = LogWriter.Instance;
            thinkTimeBetweenRequests = _thinkTimeBetweenRequests;
            _clientId = clientId;
        }


        /// <summary>
        /// ToDo: add pacing and think time parameters here.
        /// This will process script01 sequentially for as long as the main
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="method"></param>
        /// <param name="body"></param>
        /// <param name="requests"></param>
        public void SendRequest(Script script, Req req)
        {

            var method = req.method;
            var uri = req.uri;
            var request = new RestRequest(uri, method);
            req.reqStartTime = DateTime.Now;

            if (client.CookieContainer == null)
            {
                client.CookieContainer = new System.Net.CookieContainer();
            }

            if (req.method == Method.POST)
            {
                request.AddJsonBody(req.body);
            }

            // modifiy request based on  correlated value
            if (req.useExtractedText == true)
            {
                string keyName = req.nameForCorrelatedVariable;
                req.uri = req.uri.Replace("Corrolated Value Not Initialized", script.correlationsDict[keyName]);
            }

            Response response = new Response();
            Stopwatch sw = Stopwatch.StartNew();

            var dtNow = DateTime.Now;
            response.requestTimeSent = dtNow;

            if (responseIdForConcurrentDict == -1)
            {
                testStartTime = dtNow;
            }

            IRestResponse result = null;
            try
            {
                result = client.Execute(request, request.Method);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                response.responseExceptionThrown = true;
                response.responseExceptionMessage = msg;
            }
            sw.Stop();

            response.clientId = _clientId;
            response.responseId = responseIdForConcurrentDict;
            response.responseTtlb = sw.ElapsedMilliseconds;
            response.responseTimeReceived = DateTime.Now;
            response.responseStatus = "Finished";
            response.responseExceptionMessage = result.ErrorMessage;
            response.responseIdForCurrentClient = responseIdForCurrentClient++;
            response.responseStatsCode = result.StatusCode.ToString();

            ResponseDb.conCurResponseDict.TryAdd(Interlocked.Increment(ref responseIdForConcurrentDict), response);

            TimeSpan duration;
            duration = DateTime.Now - testStartTime;
            double throughPut = Interlocked.Increment(ref responseIdForLog) / duration.TotalSeconds;

            //if (responseId > 15)
            //{
            //    // we can get the time embeded in the request
            //    // the idea here is to wait until we have a few requests.
            //    duration = DateTime.Now - conCurResponseDict[responseId - 10].responseTimeReceived;
            //    throughPut = 10 / duration.TotalSeconds;
            //}


            if (responseIdForLog % 25 == 0 || responseIdForLog == 1)
            {
                writer.WriteToLog(" TtlResps \tClntId \tClntRespId \tTTLB \tThrds \tRPS \tVerb \tURI");
                writer.WriteToLog(" ======== \t====== \t========== \t==== \t===== \t=== \t==== \t===");
            }                

            writer.WriteToLog(" " + responseIdForLog
              + "\t\t" + response.clientId
              + "\t\t" + response.responseIdForCurrentClient
              + "\t\t" + response.responseTtlb
              + "\t\t" + numRestClients
              + "\t\t" + Math.Round(throughPut, 2)
              + "\t\t" + req.method
              + "\t\t" + req.uri
              );


            if (req.extractText == true)
            {
                // Correlation Hints:
                // 1. Copy the resultBody into https://rubular.com/
                // 2. left and right boundary basic format: (?<=  <left str>    )(.*?)(?=  < rt string> )
                // 3. Use https://onlinestringtools.com/escape-string to escape what you build in Rubular. 

                Regex regEx = new Regex(req.regExPattern);
                string extractedValue = regEx.Match(result.Content).Value;

                // place value into the correlation dictionary
                script.correlationsDict[req.nameForCorrelatedVariable] = extractedValue;
            }

            Thread.Sleep(thinkTimeBetweenRequests);


        }

    }
}
