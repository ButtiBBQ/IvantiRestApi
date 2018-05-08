using System;
using System.Net;
using Newtonsoft.Json.Linq;
using RestSharp;
using TaskFromTemplate.Classes;

namespace TaskFromTemplate.Controller
{
    public class IvantiRestController
    {
        private readonly RestAccessProperties _properties;
        private readonly string _accessToken;

        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="properties">proppertie set with all required informations about the API endpoint</param>
        /// <param name="ignoreServerCertificateValidation">disable ssl certificate validation</param>
        public IvantiRestController(RestAccessProperties properties, bool ignoreServerCertificateValidation = false)
        {
            _properties = properties;
            if (ignoreServerCertificateValidation)
            {   
                // Don't validate certificate - use only for DEVELOPMENT !!!
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            }

            _accessToken = GetAccessToken();
        }


        /// <summary>
        /// get Tocken from Identityserver
        /// for details see http://docs.identityserver.io/en/release/endpoints/authorize.html
        /// </summary>
        /// <returns></returns>
        private string GetAccessToken()
        {
            var client = new RestClient($"{_properties.IdentityServer}/connect/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined",
                $@"client_id={_properties.ClientId}&client_secret={_properties.ClientSecret}&grant_type={_properties.GrandType}&scope={_properties.Scope}&username={_properties.Username}&password={_properties.Password}",
                ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("Error getting access token");
                return null;
            }

            var tokenResponse = JObject.Parse(response.Content)["access_token"];
            return tokenResponse.ToString();
        }


        #region Rest Api Calls

        /// <summary>
        /// Create a task by task template and Package ID
        /// </summary>
        /// <param name="templateName">name of the template to use ^for creating the task</param>
        /// <param name="newTaskName">name of the new task to create</param>
        /// <param name="packageId">id of the package to link with the new task</param>
        /// <param name="errorMessage">is filled in case the new task could not be created</param>
        /// <returns></returns>
        public bool CreateTaskFromTemplate(string templateName, string newTaskName, string packageId, ref string errorMessage)
        {
            var client = new RestClient($"{_properties.DistributionApi}");
            client.AddDefaultHeader("Authorization", $"Bearer {_accessToken}");
            var request = new RestRequest("api/v1/TaskTemplates", Method.GET);

            // add name filter
            request.AddQueryParameter("$filter", $"contains(Name,'{templateName}')");
            //request.AddParameter("$filter", $"contains(Name,'{templateName}')");

            var response = client.Execute(request);

            // get the first template for this example
            dynamic template = JObject.Parse(response.Content)["value"].First;

            // create a task by overwriting the name of the template
            template["Name"] = newTaskName;
            // there needs to be a package id, add one if there isn't one in the template
            template["PackageId"] = packageId;
            // further settings like start time can be overwritten as well...

            request = new RestRequest("api/v1/Task", Method.POST);
            request.AddParameter("application/json", template, ParameterType.RequestBody);
            response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.Created)
                return true;
            
            errorMessage = JObject.Parse(response.Content)["error"]["message"].ToString();
            return false;
        }

        #endregion

    }
}
