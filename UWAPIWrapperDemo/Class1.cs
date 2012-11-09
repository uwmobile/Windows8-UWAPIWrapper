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
    class JSONRequest
    {
        public static async void requestJSONResponseWithoutQuery(string methodName, string APIKey)
        {
            //OutputField.Text = string.Empty;

            // The value of 'AddressField' is set by the user and is therefore untrusted input. If we can't create a
            // valid, absolute URI, we'll notify the user about the incorrect input.
            // Note that this app has both "Internet (Client)" and "Home and Work Networking" capabilities set,
            // since the user may provide URIs for servers located on the intErnet or intrAnet. If apps only
            // communicate with servers on the intErnet, only the "Internet (Client)" capability should be set.
            // Similarly if an app is only intended to communicate on the intrAnet, only the "Home and Work
            // Networking" capability should be set.
            Uri resourceUri;
            HttpClient httpClient = new HttpClient();

            string test = "http://api.uwaterloo.ca/public/v1/?key=cc7004c25526969882ff31eddb1d18f4&service=WatPark&output=json";
            if (!Uri.TryCreate(test, UriKind.Absolute, out resourceUri))
            {
                //rootPage.NotifyUser("Invalid URI.", NotifyType.ErrorMessage);
                return;
            }

            //Helpers.ScenarioStarted(StartButton, CancelButton);
            //rootPage.NotifyUser("In progress", NotifyType.StatusMessage);

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(resourceUri);
               
                

                string responseBodyAsText;

                responseBodyAsText = await response.Content.ReadAsStringAsync();

                JObject obj = new JObject();

                obj = JsonConvert.DeserializeObject<JObject>(responseBodyAsText);

                dynamic j = obj["response"];

                //responseBodyAsText = "{                                       \"1\":\"string1\",  \"2\": \"string2\"   }";
                //Diction   ary<string, string> dictionary = new Dictionary<string,string>();

                //MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(responseBodyAsText));
                //DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Dictionary<string,string>));
                //dictionary = ser.ReadObject(ms) as Dictionary<string, string>;
                

                Debug.WriteLine(responseBodyAsText);
                //await Helpers.DisplayTextResult(response, OutputField);

                //rootPage.NotifyUser   ("Completed", NotifyType.StatusMessage);
            }
            catch (HttpRequestException hre)
            {
                //rootPage.NotifyUser(hre.Message, NotifyType.ErrorMessage);
                //OutputField.Text = hre.ToString();
            }
            catch (TaskCanceledException)
            {
                //rootPage.NotifyUser("Request canceled.", NotifyType.ErrorMessage);
            }
            finally
            {
                //Helpers.ScenarioCompleted(StartButton, CancelButton);
            }

        }
    }
}
