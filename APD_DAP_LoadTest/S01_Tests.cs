using L6;
using NUnit.Framework;
using System.Threading.Tasks;


namespace ADP_DAP_LoadTest
{
    class S01_Tests
    {

        /// <summary>
        /// The idea here is that every request must be created on the fly
        /// so that we can use values from one request in a future request.
        /// Sending each request one at at time makes correlation possible. 
        /// In other words, don't build a second request before executing the first.
        /// Don't "pre-build" requests.
        /// </summary>
        [Test]
        public void S01_WorkloadDefinition()
        {
            S01_APD_DAP S01 = new S01_APD_DAP();
            int scriptIterations = 1;

            // This FOR loop sets the number of iterations of the below pattern
            for (int i = 1; i <= scriptIterations; i++)
            {
                S01.Req00(); 
                S01.Req01(); 
                S01.Req02();  
                S01.Req03(); 
                S01.Req04();
                S01.Req05();
                S01.Req06();
                S01.Req07();
                S01.Req08();
                S01.Req09();
                S01.Req10();
            }
        }


        [Test]
        public async Task S01_10Users()
        {
            UserController uc = new UserController();
            await Task.Run(() => uc.RampUpUsers(S01_WorkloadDefinition, newUserEvery:2000, maxUsers:10, testDurationSecs:360));

            PerformanceViolationChecker pvc = new PerformanceViolationChecker();

            var perfMetrics = pvc.CalcualteAllMetrics(SendRequests.conCurResponseDict);
            
            Assert.IsTrue(perfMetrics["totalTestDuration"] < 300,
                "Expected:Test Duration less than 2 minutes");

        }

        /// <summary>
        /// This test currently overwhelms the target application.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task S01_100Users()
        {
            UserController uc = new UserController();
          await Task.Run(() => uc.RampUpUsers(S01_WorkloadDefinition, newUserEvery: 1000, maxUsers: 100, testDurationSecs: 360));

            PerformanceViolationChecker pvc = new PerformanceViolationChecker();

            var perfMetrics = pvc.CalcualteAllMetrics();
        }


        
    }
}
