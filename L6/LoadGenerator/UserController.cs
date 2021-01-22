using L6.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace L6
{
    public class UserController
    {
        readonly LogWriter writer = LogWriter.Instance;

        private static int numThreads = 0;
        private static Stopwatch testStopWatch = new Stopwatch();

        /// <summary>
        /// Adds another user (thread) to process a new instance of the workload.
        /// </summary>
        /// <param name="newUserEvery">Time in milliseconds of the wait time before adding another thread. E.g., every three seconds.</param>
        /// <returns>A Task</returns>
        public async Task AddUsersByRampUp(Script script = null, int newUserEvery = 2000, int maxUsers = 2, long testDurationSecs = 360)
        {
            var tasksInProgress = new List<Task>();

            testStopWatch.Start();

            // this is a "while loop thread launcher." It launches a new thread every x seconds
            while ((testStopWatch.ElapsedMilliseconds < testDurationSecs * 1000) & (Interlocked.Increment(ref numThreads) <= maxUsers)) // loop as long as load test lasts. 
            {
                //writer.WriteToLog("Elapsed seconds=" + testStopWatch.ElapsedMilliseconds);
                var t = Task.Run(async () =>
                {
                   // writer.WriteToLog("Thread Launched=" + numThreads);
                    await TestPattern(script, numThreads, testDurationSecs); 
                });
                tasksInProgress.Add(t);
                Thread.Sleep(newUserEvery);
            }
            await Task.WhenAll(tasksInProgress);
        }



        /// <summary>
        /// Adds another user (thread) to process a new instance of the workload.
        /// </summary>
        /// <param name="newUserEvery">Time in milliseconds of the wait time before adding another thread. E.g., every three seconds.</param>
        /// <returns>A Task</returns>
        public async Task TestPattern(Script script, int clientId, long testDurationSecs = 360, string testType = "ByDuration") //alt "ByTestIterations"
        {
            // what we want it to do is start 1 task for the entire script, one task per user

            // we need the for each to be defined within the Action 

            // 0. While loop for user count
            // 1. create client
            // 2. create list of requests to send to client by iterating over the script.
            // pass the list to a task (thread) 
            // while loop for requests? 

            List<Req> requestList = script.requestList;

            SendRequests sr = new SendRequests(clientId: clientId, _thinkTimeBetweenRequests: 3);

            if (testType == "ByTestIterations")  // ToDo: Not sure this iterations is correct.
            {
                foreach (Req r in requestList)
                {
                    sr.SendRequest(script, r);
                }
            }
            else
            {
                do
                {
                    foreach (Req r in requestList) // ToDo: Not sure stopping exactly where I need
                    {
                        sr.SendRequest(script, r);
                    }
                } while (testStopWatch.ElapsedMilliseconds < testDurationSecs * 1000);
            }
        }


    }
}
