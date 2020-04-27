using L6;
using NUnit.Framework;
using System.Threading.Tasks;


namespace L6_DummyRestApi
{
    class L6_UnitTests_DummyRestApi
    {
             
        [Test]
        public async Task S02_DummyRest_10_Users_10Iterations()
        {
            UserController uc = new UserController();
            await Task.Run(() => uc.RampUpUsers(S02_DummyRestApi.ClickPathAndIterations, newUserEvery: 2000, maxUsers: 100, testDurationSecs: 360));

            PerformanceViolationChecker pvc = new PerformanceViolationChecker();
            var perfMetrics = pvc.CalcualteAllMetrics(SendRequests.conCurResponseDict);

            Assert.IsTrue(perfMetrics["totalTestDuration"] < 180,
                "Expected:Test Duration less than 2 minutes");
        }

        /// <summary>
        /// This test currently overwhelms the target application.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task S02_DummyRest_100_Users_10Iterations()
        {
            UserController uc = new UserController();
            await Task.Run(() => uc.RampUpUsers(S02_WorkloadDefinition_v2, newUserEvery: 1000, maxUsers: 100, testDurationSecs: 360));

            PerformanceViolationChecker pvc = new PerformanceViolationChecker();

            var perfMetrics = pvc.CalcualteAllMetrics(SendRequests.conCurResponseDict);
        }


        /// <summary>
        /// No iterations.
        /// </summary>
        public void S02_WorkloadDefinition_v2()
        {
            for (int i = 1; i <= 100; i++)
            {
                S02_DummyRestApi S02 = new S02_DummyRestApi(0);
                S02.Req00(); //get all
                S02.Req01(); // get one
                S02.Req02(); // create 
                S02.Req03(); // update
                S02.Req04(); //  delete
            }

        }


        //Assert.IsTrue(perfMetrics["exceptionsThrown"] == 0 &&
        //    perfMetrics["maxResponseTime"] < 600 &&
        //    perfMetrics["avgResponseTime"] < 500,
        //    "Expected:No Exceptions, max RT < 600, avg RT < 500");


    }
}
