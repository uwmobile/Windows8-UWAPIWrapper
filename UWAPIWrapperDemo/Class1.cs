//////////////////
// UWAPIWrapper
// Created by Elton Gao, UW Mobile Club
// This simple wrapper can be utilized for any Windows 8 / Windows Phone app
// that needs a interface to access University of Waterloo Open Data API:
// api.uwaterloo.ca
// Pre-requisite: you need to import Json .Net framework(Newtonsoft.Json)

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
    interface UWAPIWrapperCallback
    {
        void responseFromGenericRequest(JObject obj);
    }

    class GenericRequest
    {
        //
        // Summary:
        //     Send a GET request to the specified Uri as an asynchronous operation.
        //
        // Parameters:
        //   requestUri:
        //     The Uri the request is sent to.
        //
        // Returns:
        //     Returns System.Threading.Tasks.Task<TResult>.The task object representing
        //     the asynchronous operation.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The requestUri was null.
        public static async void requestJSONResponseWithoutQuery(string methodName, string APIKey, UWAPIWrapperCallback callbackHandler)
        {
            JObject result = null;

            //Used to store the URL
            Uri resourceUri;
            HttpClient httpClient = new HttpClient();

            string test = "http://api.uwaterloo.ca/public/v1/?key=cc7004c25526969882ff31eddb1d18f4&service=WatPark&output=json";
            if (!Uri.TryCreate(test, UriKind.Absolute, out resourceUri))
            {
                Debug.WriteLine("ERROR! Invalid request, please check your parameter");
                return;
            }

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(resourceUri);

                string responseBodyAsText;

                responseBodyAsText = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<JObject>(responseBodyAsText);
            }
            catch (HttpRequestException)
            {
                Debug.WriteLine("WARNING! Received HttpRequestException");
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("WARNING! Request is cancelled");
            }
            finally
            {
                //Anything to do here?
            }
            callbackHandler.responseFromGenericRequest(result);
        }
    }
}
