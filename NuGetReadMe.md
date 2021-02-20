## L6 - A DevOps CI/CD Performance Test Tool   

L6 is a C# .Net Core lightweight load generator designed to be used in CI pipelines.   
L6 began life as replacement for the deprecated Visual Studio Load Test tool.   
The idea is to execute multi-threaded performance/load tests as easily as functional tests, and do this in a build or release pipeline.   
L6 launches multiple threads, but in the manner of a functional, MSTest or NUnit test. It generates load using standard DevOps functional test agent machines.   

See the full documetation here: https://github.com/charlesdwmorrison/L6   

### Features/Components  
- .Net 5.0 (L6 can be executed on Linux or a Mac).   
- Correlation so that you can "chain" requests together or build user flows.   
- Ability to test more than one URL or endpoint in the test. Scripts can consist of multiple URLs.   
- A script (a list of different requests) can be built into a user flow of any length.   
- Pass/Fail results of your load test via NUnit or MSTest assertions.  Instant results in a pipeline.   
- High-performance logging class to log response time, throughput and other metrics to a text file.   
- Perf Metrics class to calculate response time average, percentiles and throughput.   
- Scripts written in familiar C# code, not JavaScript.   
- Do not have to leave Visual Studio to create a script.   
- Open Source and easily customizable.   

More than just a single URL test tool, L6 scripts consist of multiple URLs similar to LoadRunner, Visual Studio, JMeter, K6, etc.
This enables L6 to execute real user flows (scripts). 

Correlation (using the response of one request as data for the next) is accomplished by means of regular expressions. 

Correlation RegEx Syntax:

```
(?<=  <left boundary> )(.*?)(?=   < right boundary> ) 
```
### Users (Threads)
UserController.cs launches threads (in the form of C# tasks).   
"AddUsersByRampUp()" takes any script and launches and passes it to a thread every X number of seconds.

### Test Execution
To make a multi-user test, you just put the above two components together. You call the BuildRequestList() method of the script and pass the script object to the user controller: Then add an assertion:

```
[Test]
public async Task S02_OnlineRest_3_Users()
{
    // Arrange
    S02_OnlineRestExampleScript onlineRestExampleScript = new S02_OnlineRestExampleScript();
    List<Req> requestList = onlineRestExampleScript.BuildRequestList();

    // Act
    UserController uc = new UserController();
    await Task.Run(() => uc.AddUsersByRampUp(script: onlineRestExampleScript, newUserEvery: 10000, maxUsers: 3, testDurationSecs: 30));

    // Assert
    PerfMetrics pm = new PerfMetrics();
    Dictionary<string,double> perfMetrics = pm.CalcualteAllMetrics(ResponseDb.conCurResponseDict);
    Assert.IsTrue(perfMetrics["avgResponseTime"] < 3, "Expected:Avg. Response time < 3 seconds");
}
```
As you can see from the above, L6 also has a PerfMetrics class which performs calculations on the results and which you can assert against to determine if the test passed or failed.

There is also a RestSharp client class which sends the requests and performs the correlations defined in the script.

### Planned Enhancements   
- GUI with a chart to show response time and throughput. This will be done as Blazor WebAssembly Progressive Web App.   
- Examples showing how to use a CSV file as data input.   
- Script sample of non-REST application.    
- Examples of how to assert against a particular URL within a script.   
- Run multiple scripts at the same to create a load scenario of multiple user flows.   
- Create scripts programmatically by importing .har files.   
- Create a console version in order to execute from the command line. (I'm not 100% sure this is a goal.)   
- Test Agents to use more than one machine (this is a distant goal).   
