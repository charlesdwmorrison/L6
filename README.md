# L6
L6 is a C# .Net Core console-based load generator. It can be used in CI pipelines and is a replacement for the deprecated Visual Studio Load Test tool.
The idea is to execute multi-threaded performance/load tests as easily as functional tests.
Advancements over a tool like Netling include:
- .Net core (this could be run on Linux)
- Correlation so that you can add data.

##Features
---------
- .Net Core
- RestSharp 
- High performance logging class to logs response time, throughput and other metrics to a text file.
- Perf Metrics class to calculate response time average, percentiles and throughput.
- Assertions can be done with MSTest, NUnit or any other testing framework.

L6 has "scripts" just like LoadRunner or Visual Studio load tests.
Scripts in L6 are classes:

    public class S02_OnlineRestExampleScript : Script
    {
        public  List<Req> BuildRequestList()
        {
            requestList = new List<Req>()
              {
                new Req()
                {
                    uri = urlPrefix + "/employees",
                    method = Method.GET,
                    extractText = true,
                    nameForCorrelatedVariable = "empId",           
                    regExPattern = "(?<={\"id\":\")(.*?)(?=\",\"employee_name)"
                },
                new Req()
                {
                    method = Method.GET,
                    // Example of using correlated value from above
                    useExtractedText = true, // instructs SendRequest() to use a correlated value
                    nameForCorrelatedVariable = "empId",
                    uri = urlPrefix + "/employee/" + correlationsDict["empId"]
                },
                new Req
                {
                    uri = urlPrefix + "/create",
                    method = Method.POST,
                    body = "{\"name\":\"test\",\"salary\":\"123\",\"age\":\"23\"}"
                },
                 new Req
                {
                    uri = urlPrefix + "/update/1",
                    method = Method.PUT,
                    body = "{\"name\":\"test\",\"salary\":\"123\",\"age\":\"23\"}"
                },
                new Req
                {
                    uri = urlPrefix + "/delete/2",
                    method = Method.DELETE,
                }
            };

            return requestList;
        }
    }

A script object is passed to a user controller class, whcih launches threads. "AddUsersByRampUp()" takes any script, then launches a new instance of it every X number of seconds:

        public async Task AddUsersByRampUp(Script script = null, int newUserEvery = 2000, int maxUsers = 2, long testDurationSecs = 360)
        {
            var tasksInProgress = new List<Task>();
            testStopWatch.Start();
            while ((testStopWatch.ElapsedMilliseconds < testDurationSecs * 1000) & (Interlocked.Increment(ref numThreads) <= maxUsers)) // loop as long as load test lasts. 
            {
                var t = Task.Run(async () =>
                {
                    await TestPattern(script, numThreads, testDurationSecs); 
                });
                tasksInProgress.Add(t);
                Thread.Sleep(newUserEvery);
            }
            await Task.WhenAll(tasksInProgress);
        }

To make a multi user test, you just put the above two components together. You call the the "BuildRequestList()" method of the script and pass it to the Usercontroller.

        [Test]
        public async Task S02_OnlineRest_10_Users_10IterationsAsync()
        {
            S02_OnlineRestExampleScript onlineRestExampleScript = new S02_OnlineRestExampleScript();
            List<Req> requestList = onlineRestExampleScript.BuildRequestList();

            UserController uc = new UserController();
            await Task.Run(() => uc.AddUsersByRampUp(script: onlineRestExampleScript, newUserEvery: 10000, maxUsers: 3, testDurationSecs: 30));

            PerfMetrics pvc = new PerfMetrics();
            Dictionary<string, double> perfMetrics = pvc.CalcualteAllMetrics(ResponseDb.conCurResponseDict);

            Assert.IsTrue(perfMetrics["totalTestDuration"] < 180, "Expected:Test Duration less than 2 minutes");
        }

And as you can see from the above, there is a PerfMetrics class which performs calculations on the results.

There is a class to send the requests which is a simple RestSharp client. The sent request class also helps with correlation. 


##Future Enhancements
-------------------
- Be able to import .har files and create a script automatically. 
- GUI with a chart to show response time and throughput. This will be done as Blazor Progressive Web App
- Run multiple scripts at the same to create a scenario.
