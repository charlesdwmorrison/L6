# L6
L6 is a C# .Net Core load generator. L6 supports correlation and is a replacement for the deprecated Visual Studio Load Test too.

The big improvement of L6 compared to PhoenixRunner is the ability to do correlation. To get correlation I had to build a new load generator.

In PhoenixRunner, the requests were "Pre-built" and stuffed into a collection before any of them were sent. With this approach, PhoenixRunner  could add new data to each request, but it could not look at the response from one request and use a piece of the data in the next request. The requests were all "pre-built" prior to sending and could not use data from a previous response.

In L6 (I have borrowed the name from the load tool "K6"), the requests are NOT pre-built. Each request is built only after the previous request has been sent.  In this way, I can grab parts of the response and use it in the next request.

Rather than a list of requests, L6 pre-creates a list of methods to execute in the future. If you look at one of L6's "scripts" , each request in the script is actually a method. Each of these methods builds a different request. No request is built until the previous request has completed. 

L6 still uses collections and C# task. However, to make a long-running load test, L6 does not make a collection of static requests; it instead makes a collection of methods to execute.  Each C# task (thread) per user builds a new collection of methods to execute.

When a task (thread) is iterating over it's collection of methods in a script, that particular script is "single threaded." That is, one C# task is assigned to one instance of the script. Each script is a C# class.

To get multithreading going, we make a new instance of the script. Only one thread will every be assigned to that particular instance. (To enable multi-threading, we can have multiple instances of that script object, and multiple tasks (threads) running at the same time.)

L6 has "scripts" just like LoadRunner or Visual Studio web tests have scripts.
Scripts in L6 are classes (that is, they are their own objects, their own types). Requests are methods in the script class. To execute requests you instantiate an instance of the script and then call it's methods (which are requests) like this:

Script01 script01 = new Script01();
script01.Req00(); // build and send first request
script01.Req01() // build and send 2nd request
script01.Req02();
etc. 

To start a new user L6 creates a new instance of the script, and then we just assign that to a thread. A short-hand way of illustrating what L6 is doing is we are just wrapping the above code in a task:

Task t = new Task
{
Script01 script01 = new Script01();
script01.Req00(); // build and send first request
script01.Req01() // build and send 2nd request
script01.Req02();
etc. 
   }
t.Start();

 A script class also contains it's own HTTP client and this also gets built when instantiating an instance of of the script. (So this means there is one client per thread.)

There is also a new thread launcher in L6.  This is in the "UserController" class and is called "RampUpUsers."
It just simply launches a new task every X number of seconds.
This is similar to PhoenixRunner, but it is simpler than PhoenixRunner:

        public async Task RampUpUsers(Action act=null, int newUserEvery=2000, int maxUsers=2, long testDurationSecs=360 )
        {
            var tasksInProgress = new List<Task>();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while ((sw.ElapsedMilliseconds < testDurationSecs * 1000) & (numThreads < maxUsers)) // loop as long as load test lasts. 
            {
                Thread.Sleep(newUserEvery);
                Interlocked.Increment(ref numThreads);
                
                var t = Task.Run(() => act());
                tasksInProgress.Add(t);
            }
            await Task.WhenAll(tasksInProgress);
        }

In the future, we might have to make this thread launcher use it's own threadpool (we have to see if the default threadpool can keep up. 
