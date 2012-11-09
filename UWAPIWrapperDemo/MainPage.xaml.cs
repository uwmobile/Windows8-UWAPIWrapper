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
            UWAPIWrapper.GenericRequest.requestDataInJSONWithoutQuery("WatPark", "cc7004c25526969882ff31eddb1d18f4", responseFromGenericRequest, null);
            UWAPIWrapper.GenericRequest.requestDataInJSONWithQuery("CS 486", "CourseSearch", "cc7004c25526969882ff31eddb1d18f4", responseFromGenericRequest, null);
        }

        public void responseFromGenericRequest(string methodName, JObject obj)
        {
            Debug.WriteLine(obj.ToString());
        }
    }
}
 