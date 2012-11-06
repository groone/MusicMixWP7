using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace JukebotWP7.Models
{
    public class JukebotFsqVenue 
    {
        public string id { get; set; }
        public FsqLocation location { get; set; }
        public string name { get; set; }
        public bool HasJukebot { get; set; }
    }

#region imported Jukebot.Data.JsonFormats.FsqFormats

    public class FsqVenue
    {
        public string id { get; set; }
        public string name { get; set; }
        public FsqLocation location { get; set; }

        //venues: [            {
        //        id: "50940952e4b0a40e1b875d24"
        //        name: "Viestura prospekts 11"
        //        contact: { }
        //        location: { FsqLocation }
        //        categories: [ { FsqCategory } ]
        //        verified: true
        //        stats: {
        //            checkinsCount: 1
        //            usersCount: 1
        //            tipCount: 0
        //        }
        //        likes: {
        //            count: 0
        //            groups: [ ]
        //        }
        //        specials: {
        //            count: 0
        //            items: [ ]
        //        }
        //        venuePage: {
        //            id: "40622872"
        //        }
        //        canonicalUrl: "https://foursquare.com/v/viestura-prospekts-11/50940952e4b0a40e1b875d24"
        //    }
        //]


    }

    public class FsqLocation
    {
        public string address { get; set; }
        public double lat { get; set; }
        public double lng { get; set; }
        public double distance { get; set; }  // or is it int?
        public string city { get; set; }
        public string country { get; set; }
        public string cc { get; set; }

        //"location": {"address": "Viestura prospekts 11",
        //  "lat": 56.9974,
        //  "lng": 24.1308,
        //  "distance": 4,
        //  "city": "Riga",
        //  "country": "Latvia",
        //  "cc": "LV"
        //}
    }

#endregion
}
