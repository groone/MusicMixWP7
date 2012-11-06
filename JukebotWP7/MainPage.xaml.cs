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
using System.Windows.Navigation;

namespace JukebotWP7
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                if (!String.IsNullOrEmpty(((App)App.Current).Token))
                {
                    uxLogin.Visibility = System.Windows.Visibility.Collapsed;
                    NavigationService.Navigate(new Uri("/VenueList.xaml", UriKind.Relative));
                    //NavigationService.RemoveBackEntry();
                }
            }
        }

        private void uxLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Login.xaml", UriKind.Relative));
        }

    }
}