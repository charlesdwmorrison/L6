using L6;
using L6.Models;
using RestSharp;
using System.Collections.Generic;

namespace L6_OnlineRestExample
{
    public class S02_OnlineRestExampleScript : Script
    {

        // http://dummy.restapiexample.com  is a live, online example rest service. 
        private static string urlPrefix = "http://dummy.restapiexample.com/api/v1";

    
        /// <summary>
        /// Note: ToDo: So far, we make new instances of this script because.
        ///       each client has it's own version of the script. Not 100% sure this is the best way. 
        /// </summary>
        /// <returns></returns>
        public  List<Req> BuildRequestList()
        {
            // Register correlations by adding another element to this dictionary.
            // For each correlated value we must register the key name for that value.
            // This dictionary is inherted from class "Script". 
            // ToDo: This is not completely kosher that we build the dictionary in the list. 
            correlationsDict.Add("empId", "Corrolated Value Not Initialized");

            requestList = new List<Req>()
              {
                new Req()
                {
                    uri = urlPrefix + "/employees",
                    method = Method.GET,
                    extractText = true,
                    nameForCorrelatedVariable = "empId",

                    // Correlation Tips:
                    // 1. Copy the resultBody into https://rubular.com/
                    // 2. left and right boundary basic format: (?<=  <left str>    )(.*?)(?=  < rt string> )
                    // 3. Use https://onlinestringtools.com/escape-string to escape what you build in Rubular. 
                    // Loooking for: [{"id":1,"employee_name":"Tiger              
                    regExPattern = "(?<={\"id\":)(.*?)(?=,\"employee_name)"
                },
                new Req()
                {
                    method = Method.GET,
                    // Example of using correlated value from above
                    //uri = urlPrefix + "/employee/1", // original
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
}
