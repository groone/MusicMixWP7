using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace JukebotWP7
{
    public partial class Login : PhoneApplicationPage
    {
        string CLIENT_ID = "GP0WCDFME4UVNKEU1EVSUKKI0HPGEPVJDHEA2JRCT03OEBNT";
        string CALLBACK_URI = "http://jukebot.co/Venue/ExternalLoginCallback";

        public Login()
        {
            InitializeComponent();

            //https://foursquare.com/oauth2/authenticate
            //?client_id=CLIENT_ID
            //&response_type=token
            //&redirect_uri=YOUR_REGISTERED_REDIRECT_URI

                string address =
            "https://foursquare.com/oauth2/authorize" +
            "?client_id=" + CLIENT_ID +
            "&response_type=token" +
            "&redirect_uri=" + CALLBACK_URI;

            uxBrowser.Navigating += new EventHandler<NavigatingEventArgs>(uxBrowser_Navigating);
            uxBrowser.Navigate(new Uri(address));
        }

        void uxBrowser_Navigating(object sender, NavigatingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("navigating: " + e.Uri);
            if (e.Uri.ToString().StartsWith(CALLBACK_URI))
            {
                e.Cancel = true;
                var response = e.Uri.ToString();
                if (response.LastIndexOf("access_token=") != -1)
                {
                    string token = response.Substring(response.LastIndexOf("#access_token=") + "#access_token=".Length);
                    System.Diagnostics.Debug.WriteLine("TOKEN: " + token);
                    ((App)App.Current).Token = token;
                }
                NavigationService.GoBack();
            }
        }

    }
}