///////////////////////////////////////////////////////////////////////////////////////////////
// UWAPIWrapper
// Created by Elton Gao, UW Mobile Club
// This simple wrapper can be utilized for any Windows 8 / Windows Phone app
// that needs a interface to access University of Waterloo Open Data API:
//              api.uwaterloo.ca
// Pre-requisite: you need to import Json .Net framework(Newtonsoft.Json)
// To do this: Right click you project on the solution explorer, select "Manage NuGet Packages"
//             Wait for list of packages online, when done, select JSON .Net from the list.
//             Now you should be able to import this:
//                  using Newtonsoft.Json;
//                  using Newtonsoft.Json.Linq;
// To Use this wrapper: 
// invoke method:
//     UWAPIWrapper.GenericRequest.requestDataInJSONWithQuery
//          (<query>,<method_name>, <your_api_key>, <optional, method_name_you_implement_for_handling_data>, <optional, method_name_you_implement_for_handling_fail>);
//OR
//     UWAPIWrapper.GenericRequest.requestDataInJSONWithoutQuery
//          (<method_name>, <your_api_key>, <optional, method_name_you_implement_for_handling_data>, <optional, method_name_you_implement_for_handling_fail>);
//
// Github repo: <link here>
// Let's build this wrapper together by sending pull request to me!
// Open sourced project, feel free to do anything with this, No attribution required
///////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UWAPIWrapper
{
    static class UWAPIConstants
    {
        public const string UW_API_BASE_URL = "http://api.uwaterloo.ca/public/v1/?";
    }

    public class GenericRequest
    {
        //Summary:
        //  List of delegate you can implement in your part

        //onGetResponseFromRequest
        //Returns:
        //  Parsed JSON response in JObject Format
        //  it will not be null
        //  JObject is just a dictionary or array
        //  For UW API, first level is all dictionary
        //  So you can do obj["response"] to get the value for key "response"
        public delegate void onGetResponseFromRequest(GenericRequest request, string methodName, JObject obj);

        //onFailToGetResponse
        //Returns:
        //  Exception if there is any
        //  Or null if no info available
        public delegate void onFailToGetResponse(GenericRequest request, string methodName, Exception E);

        //
        // Summary:
        //     Give a method name and API key, it will return the parsed JSON object
        //
        // Parameters:
        //   query:
        //     For some API call, we need a query, you can set null or call another method to avoid this param
        //
        //   methodName:
        //     Name of the method you specify
        //
        //   APIKey:
        //      API key you get from api.uwaterloo.ca
        //
        //   completionHandler:
        //      optional, but what's the point of not having this?
        //      method name for processing the returned data when you get it
        //      method must have the parameter specified as the delgate method
        //
        //      NOTE!!! returned JObject may not be valid when API key is invalid
        //
        //   failHandler:
        //      optional
        //      method name which will be called by this if anyting goes wrong
        //      method must have the parameter specified as the delgate method
        //
        // Returns:
        //     none
        //
        // Exception:
        //     None for now

        public async void requestDataInJSONWithQuery(string query, string methodName, string APIKey, onGetResponseFromRequest completionHandler, onFailToGetResponse failHandler)
        {
            JObject result = null;

            //Used to store the URL
            Uri resourceUri;
            HttpClient httpClient = new HttpClient();

            string test = UWAPIConstants.UW_API_BASE_URL + "key=" + APIKey + "&service=" + methodName + "&output=" + "json";
            if (query != null)
            {
                test += "&q=" + query;
            }

            if (!Uri.TryCreate(test, UriKind.Absolute, out resourceUri))
            {
                Debug.WriteLine("ERROR! Invalid request, please check your parameter");
                if (failHandler != null)
                {
                    failHandler(this, methodName, null);
                }
                return;
            }

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(resourceUri);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Debug.WriteLine("INFO -- Response Code: {0}", response.StatusCode);
                }

                string responseBodyAsText;

                responseBodyAsText = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<JObject>(responseBodyAsText);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine("WARNING! Received HttpRequestException");
                if (failHandler != null)
                {
                    failHandler(this, methodName, e);
                }
            }
            catch (TaskCanceledException e)
            {
                Debug.WriteLine("WARNING! Request is cancelled");
                if (failHandler != null)
                {
                    failHandler(this, methodName, e);
                }
            }
            finally
            {
                //Anything to do here?
            }

            if (completionHandler != null && result != null)
            {
                completionHandler(this, methodName, result);
            }
            else
            {
                Debug.WriteLine("WARNING! Failed to parse JSON object");
                if (failHandler != null)
                {
                    failHandler(this, methodName, null);
                }
            }
        }

        public async void requestDataInJSONWithoutQuery(string methodName, string APIKey, onGetResponseFromRequest completionHandler, onFailToGetResponse failHandler)
        {
            this.requestDataInJSONWithQuery(null, methodName, APIKey, completionHandler, failHandler);
        }

    }
}
