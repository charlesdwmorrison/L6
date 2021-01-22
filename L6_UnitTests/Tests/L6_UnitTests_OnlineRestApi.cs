using L6;
using L6.LoadGenerator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace L6_OnlineRestExample
{
    class S02_OnlineRestExample
    {

        static int perfViolationChkCount = 0;


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



    }
}
