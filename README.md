# L6 - A DevOps CI/CD Performance Test Tool

L6 is a C# .Net Core lightweight load generator designed to be used in CI pipelines.   
L6 began life as replacement for the deprecated Visual Studio Load Test tool.   
The idea is to execute multi-threaded performance/load tests as easily as functional tests, and do this in a build or release pipeline.  
L6 launches multiple threads, but in the manner of a functional, MSTest or NUnit test. It generates load using standard DevOps functional test agent machines.  

Advancements over vernerable tools such as [Netling](https://github.com/hallatore/Netling) include:
- .Net core (L6 can be executed on Linux or a Mac).
- [Correlation](http://www.methodsandtools.com/archive/loadtesterrors1.php) so that you can "chain" requests together or build user flows.
- Ability to test more than one URL or endpoint in the test. Scripts can consist of multiple URLs. 
- A script (a list of different requests) can be built into a user flow of any length.
- L6 provides a pass/fail result depending on service level agreements set by the tester. 

As a CI/CD tool, L6 is an easy way to execute multiple threads against an application in order to catch (in an automated, DevOps manner) multi-threading bugs, such as improperly locked variables, [true scalability](https://essentialcomputing.wordpress.com/2016/11/13/a-practical-contribution-to-the-meaning-of-scalability-measuring-code-scalability/), and, of course, response time.  
Since L6 allows you to easily execute multiple threads, it will also allow you to investigate your database [indexes](https://www.red-gate.com/simple-talk/sql/performance/tune-your-indexing-strategy-with-sql-server-dmvs/) (using the [QueryStore](https://www.red-gate.com/simple-talk/sql/database-administration/the-sql-server-2016-query-store-accessing-query-store-information-using-dmvs/), [compilation times](https://essentialcomputing.wordpress.com/2016/10/06/measuring-compilation-time-in-sql-server/) and [deadlocks](https://www.red-gate.com/simple-talk/sql/database-administration/handling-deadlocks-in-sql-server/).
L6 therefore helps to "shift left" with performance testing, making the process of performance/load testing much more agile. 
L6 can also be used as a desktop load generator to execute heavier loads. 

## Features/Compoents
- .Net Core 5.0
- RestSharp 
- High-performance logging class to log response time, throughput and other metrics to a text file.
- Perf Metrics class to calculate response time average, percentiles and throughput.
- Assertions can be performed against response time or thorughput using MSTest, NUnit or any other testing framework.
- Do not have to leave Visual Studio to create a script. 
- Easier to understand script syntax compared to [K6](https://medium.com/swlh/beginners-guide-to-load-testing-with-k6-ff155885b6db) or some other tools. 
- Scripts written in familiar C# code, not JavaScript.
- Open Source and easily customizable.

## Usage
### Scripts
More than just a single URL test tool, L6 scripts consist of multiple URLs just like LoadRunner or Visual Studio, JMeter, K6, etc. This enables L6 to execute real user flows (scripts).
Currently, we must make the scripts by hand (see future enhancements below). But creatiing a script is easy. You just add the requests to a C# List<> collection.

Scripts in L6 are classes with one method, BuildRequestList(). BuildRequestList() returns a list of the requests you want to execute.

```
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
```

Correlation (using the response of one request as data for the next) is accomplished by means of regular expressions.  You can see an example of it in the above script. 

Correlation RegEx Syntax:  
```
(?<=  <left boundary> )(.*?)(?=   < right boundary> )  
``` 
    
### Users (Threads)
A script object is passed to a user controller class, which launches threads (in the form of C# tasks).
"AddUsersByRampUp()" takes any script, then launches a new instance of it every X number of seconds:

```
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
```
### Test Execution
To make a multi-user test, you just put the above two components together.
You call the BuildRequestList() method of the script and pass the script object to the user controller. Then add an assertion:

```
[Test]
public async Task S02_OnlineRest_3_Users()
{
    S02_OnlineRestExampleScript onlineRestExampleScript = new S02_OnlineRestExampleScript();
    List<Req> requestList = onlineRestExampleScript.BuildRequestList();

    UserController uc = new UserController();
    await Task.Run(() => uc.AddUsersByRampUp(script: onlineRestExampleScript, newUserEvery: 10000, maxUsers: 3, testDurationSecs: 30));

    PerfMetrics pm = new PerfMetrics();
    Dictionary<string,double> perfMetrics = pm.CalcualteAllMetrics(ResponseDb.conCurResponseDict);

    Assert.IsTrue(perfMetrics["avgResponseTime"] < 3, "Expected:Avg. Response time < 3 seconds");
}
```

As you can see from the above, L6 also has a PerfMetrics class which performs calculations on the results and which you can assert against to determine if the test passed or failed.

There is also a RestSharp client class which sends the requests and performs the correlations defined in the script.


## Planned Enhancements
- GUI with a chart to show response time and throughput. This will be done as [Blazor WebAssembly Progressive Web App](https://devblogs.microsoft.com/visualstudio/building-a-progressive-web-app-with-blazor).
- Examples showing how to use a CSV file as data input.
- Script sample of non-REST application.
- Examples of how to assert against a particular URL within a script. 
- Run multiple scripts at the same to create a load scenario of multiple user flows.
- Create scripts programmatically by importing .har files.
- Create a console version in order to execute from the command line. (I'm not 100% sure this is a goal.)
- Test Agents to use more than one machine (this is a distant goal).
