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
using System.Device.Location;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using JukebotWP7.Models;

namespace JukebotWP7
{
    public partial class VenueList : PhoneApplicationPage
    {
        private GeoCoordinateWatcher watcher;

        public VenueList()
        {
            InitializeComponent();

            // search for foursquare venues
            // get coordinates
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.MovementThreshold = 1;

            // Add event handlers for StatusChanged and PositionChanged events
            watcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(watcher_StatusChanged);
            watcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watcher_PositionChanged);

            // Start data acquisition
            watcher.Start();

            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationService.RemoveBackEntry();
        }

        private void watcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                    // location is unsupported on this device
                    System.Diagnostics.Debug.WriteLine("GeoPositionStatus:Disabled");
                    break;
                case GeoPositionStatus.NoData:
                    // data unavailable
                    System.Diagnostics.Debug.WriteLine("GeoPositionStatus:NoData");
                    break;
            }
        }

        void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            var loc = e.Position.Location;
            System.Diagnostics.Debug.WriteLine("Accuracy info: Horizontal:{0} Timestamp:{1}", loc.HorizontalAccuracy, e.Position.Timestamp);
            //if (loc.HorizontalAccuracy < 500)  // could break compatibility with some devices.
            //{
                watcher.Stop();
                GetJukebotVenueList(loc.Latitude, loc.Longitude);
            //}
        }

        #region venues from foursquare
        private void GetVenueList(double latitude, double longitude)
        {
            string requestURI = String.Format("https://api.foursquare.com/v2/venues/search?ll={0},{1}&intent=checkin&limit=10&oauth_token={2}&v=20121029", latitude, longitude, ((App)App.Current).Token);
            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri(requestURI));
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            
            var jObj = JObject.Parse(e.Result);
            System.Diagnostics.Debug.WriteLine(jObj.ToString());
            var venues = GetVenues(jObj);
            uxVenueList.ItemsSource = venues;
        }

        //delete
        private List<JukebotFsqVenue> GetVenues(JObject jObj)
        {
            List<JukebotFsqVenue> list = null;

            var venues = jObj["response"]["venues"];

            list = venues.Select(x => new JukebotFsqVenue() 
                { 
                    name = x["name"].ToString(),
                    id = x["id"].ToString(),
                }).ToList();
            return list ?? new List<JukebotFsqVenue>(0);
        }
        #endregion

        private void GetJukebotVenueList(double latitude, double longitude)
        {
            string requestURI = "http://jukebot.co/device/FindVenues";
            string requestPOST = String.Format("Token={0}&Latitude={1}&Longitude={2}", ((App)App.Current).Token, latitude, longitude);
            WebClient webClientJukebot = new WebClient();
            webClientJukebot.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            webClientJukebot.Headers[HttpRequestHeader.ContentLength] = requestPOST.Length.ToString();
            webClientJukebot.UploadStringCompleted += new UploadStringCompletedEventHandler(webClientJukebot_UploadStringCompleted);
            webClientJukebot.UploadStringAsync(new Uri(requestURI), "POST", requestPOST);
        }

        void webClientJukebot_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("GetJukebotVenueList response: " + e.Result);
            var jObj = JArray.Parse(e.Result);
            List<Models.JukebotFsqVenue> venueList = jObj.Select(x => x.ToObject<Models.JukebotFsqVenue>()).ToList();
            uxVenueList.ItemsSource = venueList;
        }

        protected void uxVenueList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            if (lb.SelectedItem != null)
            {
                var current = lb.SelectedItem as JukebotFsqVenue;
                System.Diagnostics.Debug.WriteLine("Checking in at " + current.name);
                ((App)App.Current).CurrentVenue = current;
                NavigationService.Navigate(new Uri("/Venue.xaml", UriKind.Relative));
            }

            lb.SelectedIndex = -1;
        }
    }

}