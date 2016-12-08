using System;


namespace CrossGeofenceSample.Models
{
    public class Place
    {
        public Place()
        {
        }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Loongitude { get; set; }
        public double Radius { get; set; }
    }
}
