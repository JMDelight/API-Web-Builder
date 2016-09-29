using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SiteBuilderAPI.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SiteBuilderAPI.Controllers
{
    [Route("api/[controller]")]
    public class WebSiteController : Controller
    {
        // GET: api/values
        [HttpGet]
        public Dictionary<string, string> Get()
        {
            var client = new RestClient("https://dotnet-test-4a976.firebaseio.com/");
            //2
            var request = new RestRequest("websites.json", Method.GET);
            //3
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            //var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());
            List<string> keys = new List<string>();
            foreach(var x in jsonResponse)
            {
                string key = x.Key;
                keys.Add(key);
            }
            var abc = jsonResponse["-KSm-bIZURFnsKxHCTzL"]["contents"];
            Dictionary<string, string> websites = new Dictionary<string, string>();
            List<string> siteNames = new List<string>();
            foreach (string str in keys)
            {
                string title = (string) jsonResponse[str]["title"];
                siteNames.Add(title);
                websites.Add(str, title);
                //string[] content = jsonResponse[str]["contents"];
            }

            string returnString = "";
            foreach(var str in abc)
            {
                returnString += str;
            }
            return websites;
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }

        // GET api/values/5
        [HttpGet("{key}")]
        public string[] Get(string key)
        {
            var client = new RestClient("https://dotnet-test-4a976.firebaseio.com/");
            //2
            var request = new RestRequest("/websites/" + key + "/contents.json", Method.GET);
            //3
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(response.Content);
            var messageList = JsonConvert.DeserializeObject<string[]>(jsonResponse.ToString());
            return messageList;
        }

        // POST api/values
        [HttpPost]
        public void Post(string Title = "not found", string Contents = "no contents")
        {
            var client = new RestClient("https://dotnet-test-4a976.firebaseio.com/");
            //2

            var request = new RestRequest("/websites/.json", Method.POST);
            request.AddHeader("Content-type", "application/json");
            //request.AddParameter("title", title);
            //request.AddParameter("contents", contents);
            string[] splitContent = Contents.Split('`');
            request.AddJsonBody(
            new
            {
                title = Title,
                contents = splitContent
            });

            //3
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            //return response;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
