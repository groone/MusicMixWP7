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
    public partial class Venue : PhoneApplicationPage
    {
        public Venue()
        {
            InitializeComponent();
            var currentVenue = ((App)App.Current).CurrentVenue;
            PageTitle.Text = currentVenue.name;
        }
        
    }
}