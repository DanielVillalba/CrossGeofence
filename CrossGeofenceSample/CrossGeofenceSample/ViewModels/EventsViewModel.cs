using System;
using PropertyChanged;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using CrossGeofenceSample.Helpers;
using System.Windows.Input;
using Xamarin.Forms;

namespace CrossGeofenceSample.ViewModels
{
    [ImplementPropertyChanged]
    public class EventsViewModel
    {
        public EventsViewModel()
        {
        }

        public ObservableCollection<Models.Event> Data { get { return App.Events; } }
    }
}
