using RestSharp;
using System.Collections.Generic;
using System.Threading;
using L6;

namespace L6_DummyRestApi
{
    public class S02_DummyRestApi
    {
        
        private static string urlPrefix = "http://dummy.restapiexample.com/api/v1";
        private SendRequests sr;
        private static int numClients = 0;

        public S02_DummyRestApi(int thinkTime)
        {

            sr = new SendRequests(thinkTime);
            LogWriter writer = LogWriter.Instance;
            Interlocked.Increment(ref numClients);
            writer.WriteToLog("# clients=" + numClients);
        }

        // correlation variables, initialized value is empty. Will fill after getting response
        //public string empId = "";
        public Dictionary<string, string> correlations = new Dictionary<string, string>
        {
            {"empId"," "}
        };

        /// <summary>
        /// The idea here is that every request must be created on the fly
        /// so that we can use values from one request in a future request.
        /// Sending each request one at at time makes correlation possible. 
        /// In other words, don't build a second request before executing the first.
        /// Don't "pre-build" requests.
        /// We are NOT constructing a list of requests;
        /// we are construction a list of *methods* which will be executed.
        /// </summary>
        public static void ClickPathAndIterations()
        {
            S02_DummyRestApi S02 = new S02_DummyRestApi(0);

            // This FOR loop sets the number of iterations of the below pattern
            for (int i = 1; i <= 1000; i++)
            {
                S02.Req00(); //get all
                S02.Req01(); // get one
                S02.Req02(); // create 
                S02.Req03(); // update
                S02.Req04(); //  delete
                             // S02.Pacing(1000);
            }
        }

        public void Req00()
        {
            Req req = new Req
            {
                uri = urlPrefix + "/employees",
                method = Method.GET,

                useExtractText = true,
               // extractText = empId,
                // looking for {"id":"10","employee_name
                // This means "find a string from the *response* to this request
                // This is just like LoadRunner correlation. 
                leftBoundary = "{\"id\":\"",
                rightBoundary = "\",\"employee_name",
                
            };
             sr.SendRequest(req);
            // assignment of the correlated value occurs here
            correlations["empId"] = req.correlatedValue;
        }


        public void Req01()
        {
            Req req = new Req
            {                
                // Example of using correlation from above in next request. 
                //uri = urlPrefix + "/employee/1", // original
                uri = urlPrefix + "/employee/" + correlations["empId"], // we only need to refer to the previous request.
                method = Method.GET,
            };
            sr.SendRequest(req);
        }


        public void Req02()
        {
            Req req = new Req
            {
                uri = urlPrefix + "/create",
                method = Method.POST,
                body = "{\"name\":\"test\",\"salary\":\"123\",\"age\":\"23\"}"
            };
            sr.SendRequest(req);
        }

        public void Req03()
        {
            Req req = new Req
            {
                uri = urlPrefix + "/update/1",
                method = Method.PUT,
                body = "{\"name\":\"test\",\"salary\":\"123\",\"age\":\"23\"}"
            };
            sr.SendRequest(req);
        }

        public void Req04()
        {
            Req req = new Req
            {
                uri = urlPrefix + "/delete/2",
                method = Method.DELETE,
            };
            sr.SendRequest(req);
        }


        public void Pacing(int pacingTimeinMs)
        {
            Thread.Sleep(pacingTimeinMs);
        }



      


    }
}
