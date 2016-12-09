using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CrossGeofenceSample.Views
{
    public partial class EventViewCell : ViewCell
    {
        public EventViewCell()
        {
            InitializeComponent();
        }

        void OnDelete(object sender, EventArgs args)
        {
            var e = (Models.Event)this.BindingContext;

            App.Events.Remove(e);
            Helpers.Settings.RemoveEvent(e);
        }
    }
}
