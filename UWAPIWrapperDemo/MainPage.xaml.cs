using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using UWAPIWrapper;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWAPIWrapperDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (query_text_box.Text.Length > 0)
            {
                GenericRequest req2 = new GenericRequest();
                req2.requestDataInJSONWithQuery(query_text_box.Text, method_name_text_box.Text, api_key_text_box.Text, responseFromGenericRequest, genericRequestFailed);
            }
            else
            {
                GenericRequest req1 = new GenericRequest();
                req1.requestDataInJSONWithoutQuery(method_name_text_box.Text, api_key_text_box.Text, responseFromGenericRequest, genericRequestFailed);
            }
        }

        public void responseFromGenericRequest(GenericRequest request, string methodName, JObject obj)
        {
            Debug.WriteLine(obj.ToString());
            output_textbox.Text = obj.ToString();
        }

        public void genericRequestFailed(GenericRequest req, string methodName, Exception e)
        {
            output_textbox.Text = "Opps! Something goes wrong!";
        }
    }
}
 