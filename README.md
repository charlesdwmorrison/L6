# L6
L6 is a C# .Net Core load generator designed to be used in CI pipelines using standard test agent machines.  
It started life as replacement for the deprecated Visual Studio Load Test tool.  
The idea is to execute multi-threaded performance/load tests as easily as functional tests.  

Advancements over vernerable tools such as [Netling](https://github.com/hallatore/Netling) include:
- .Net core (L6 can be executed on Linux or a Mac).
- Correlation so that you can add data and vary the body or URI.
- Ability to test more than one URL or endpoint at a time. Scripts consisting of multiple URLs can be used, and user flows created.
- Can be run in a DevOps pipeline.  

L6 is basically a CI/CD tool for performance testing. 

## Features
- .Net Core
- RestSharp 
- High performance logging class to log response time, throughput and other metrics to a text file.
- Perf Metrics class to calculate response time average, percentiles and throughput.
- Assertions can be performed against response time or thorughput using MSTest, NUnit or any other testing framework.

## Usage
### Scripts
L6 has "scripts" consisting of multiple URLs just like LoadRunner or Visual Studio load tests.
Currently, you have to make the scripts by hand, but its easy. You just add the requests to a C# List<> collection:

Correlation (using the response of one request as data for the next) is accomplished by means of regular expressions.  

Correlation RegEx Syntax:  
```
(?<=  <left boundary> )(.*?)(?=   < right boundary> )
```

Scripts in L6 are classes with one method, BuildRequest(). BuildRequest() returns a list of the requests you want to execute.

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

And as you can see from the above, L6 also has a PerfMetrics class which performs calculations on the results and which you can assert against to determine if the test passed or failed.

There is also a class to send the requests which is a simple RestSharp client. 
The send request class also performs the correlation specified in the script.


## Future Enhancements
- Create scripts programmatically by importing .har files. 
- GUI with a chart to show response time and throughput. This will be done as [Blazor WebAssembly Progressive Web App](https://devblogs.microsoft.com/visualstudio/building-a-progressive-web-app-with-blazor).
- Run multiple scripts at the same to create a load scenario.
- Create a console version in order to execute from the command line. (I'm not 100% sure this is a goal.)
- Test Agents to use more than one machine (this is a distant goal).
