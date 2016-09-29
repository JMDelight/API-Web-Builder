using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SiteBuilderClient.wwwroot.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            var client = new RestClient("http://localhost:65000/api");
            //2
            var request = new RestRequest("/website", Method.GET);
            //3
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            //JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            //var messageList = JsonConvert.DeserializeObject<List<Message>>(jsonResponse["messages"].ToString());

            //var abc = jsonResponse["-KSm-bIZURFnsKxHCTzL"]["contents"];
            //string returnString = "";
            //foreach (var str in abc)
            //{
            //    returnString += str;
            //}

            //List<string> keys = new List<string>();
            //foreach (var x in jsonResponse)
            //{
            //    string key = x.Key;
            //    keys.Add(key);
            //}
            ////Dictionary<string, List<string>> websites = new Dictionary<string, List<string>>();


            //foreach (string str in keys)
            //{
            //    string title = (string)jsonResponse[str]["title"];
            //    List<string> content = new List<string>();
            //    foreach (string item in jsonResponse[str]["contents"])
            //    {
            //        content.Add(item);
            //    }
            //    websites.Add(title, content);
            //    //string[] content = jsonResponse[str]["contents"];
            //}
            JObject jsonResponse = JsonConvert.DeserializeObject<JObject>(response.Content);
            Dictionary<string, string>siteNames = new Dictionary<string, string> ();
            foreach(var str in jsonResponse)
            {
                string site = str.ToString();

                string[] pairValue = site.Split(new char[] { ',' }, 2);
                siteNames.Add(pairValue[0].Substring(1), pairValue[1].Substring(0, pairValue[1].Length - 1));
            }
            //var messageList = JsonConvert.DeserializeObject<JObject>(jsonResponse.ToObject<Dictionary<string, string>>);
            //var abc = response.Content[0];
            ViewBag.siteNames = siteNames;

            return View();
        }

        public IActionResult ViewSite(string key)
        {
            var client = new RestClient("http://localhost:65000/api");
            //2
            var request = new RestRequest("/website/"+key, Method.GET);
            //3
            var response = new RestResponse();
            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();
            JArray jsonResponse = JsonConvert.DeserializeObject<JArray>(response.Content);
            var messageList = JsonConvert.DeserializeObject<string[]>(jsonResponse.ToString());
            //var abc = response.Content[0];
            ViewBag.pageHTML = messageList;

            return View();
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient theClient, RestRequest theRequest)
        {
            var tcs = new TaskCompletionSource<IRestResponse>();
            theClient.ExecuteAsync(theRequest, response => {
                tcs.SetResult(response);
            });
            return tcs.Task;
        }
    }
}
