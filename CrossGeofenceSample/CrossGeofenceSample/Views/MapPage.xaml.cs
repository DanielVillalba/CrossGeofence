using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CrossGeofenceSample.Views
{
    public partial class MapPage : ContentPage
    {
        bool isInitialized = false;
        public static readonly BindableProperty LatitudeProperty = BindableProperty.Create<MapPage, double>(p => p.Latitude, 0.0);
        public static readonly BindableProperty LongitudeProperty = BindableProperty.Create<MapPage, double>(p => p.Longitude, 0.0);

        public double Latitude
        {
            get
            {

                return (double)GetValue(LatitudeProperty);
            }
            set
            {
                SetValue(LatitudeProperty, value);


            }
        }

        public double Longitude
        {
            get
            {
                return (double)GetValue(LongitudeProperty);
            }
            set
            {
                SetValue(LongitudeProperty, value);
            }
        }

        public MapPage()
        {
            InitializeComponent();
        }
    }
}
