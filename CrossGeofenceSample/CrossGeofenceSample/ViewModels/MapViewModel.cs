﻿using System;
using PropertyChanged;
using System.Windows.Input;
using Xamarin.Forms;
using System.Threading.Tasks;
using Acr.UserDialogs;
using System.Diagnostics;
using System.Collections.ObjectModel;
using Geofence.Plugin;
using Geofence.Plugin.Abstractions;

namespace CrossGeofenceSample.ViewModels
{
    [ImplementPropertyChanged]
    public class MapViewModel
    {
        public MapViewModel()
        {
            Data = new ObservableCollection<Models.Place>();

            // restored regions
            foreach(var region in CrossGeofence.Current.Regions.Values)
            {
                var place = new Models.Place()
                {
                    Name = region.Id,
                    Latitude = region.Latitude,
                    Longitude = region.Longitude,
                    Radius = region.Radius
                };
                Data.Add(place);
            }

            IsBusy = true;
        }

        public ObservableCollection<Models.Place> Data { get; set; }

        public bool IsBusy { get; set; }

        public ICommand EditCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsEditing = true;
                    Debug.WriteLine(Latitude);
                    Debug.WriteLine(Longitude);
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsEditing = false;
                });
            }
        }

        public ICommand AddCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await AddGeofence();
                });
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                return new Command(async () =>
                {
                    bool result = await UserDialogs.Instance.ConfirmAsync("Are you sure you would like to clear all regions monitoring and events data?", "Clear All Data");
                    if (result)
                    {
                        if (CrossGeofence.Current.IsMonitoring)
                        {
                            CrossGeofence.Current.StopMonitoringAllRegions();

                            MessagingCenter.Send<MapViewModel>(this, "ClearPins");
                        }
                    }
                });
            }
        }

        public bool IsEditing { get; set; }
        public bool IsNotEditing { get { return !IsEditing && IsNotBusy; } }
        public bool IsNotBusy { get { return !IsBusy; }  }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        private async Task AddGeofence()
        {
            PromptResult result = await UserDialogs.Instance.PromptAsync("Enter geofence region name", "Add Geofence");
            if (result.Ok)
            {
                try
                {
                    PromptResult result1 = await UserDialogs.Instance.PromptAsync("Enter radius (in meters)", "Geofence Radius", "Ok", "Cancel", "", InputType.Number);
                    double radius = 50;
                    if (!string.IsNullOrEmpty(result1.Text))
                    {
                        radius = double.Parse(result1.Text);
                    }

                    UserDialogs.Instance.ShowLoading();

                    var place = new Models.Place()
                    {
                        Name = result.Text,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Radius = radius
                    };

                    Data.Add(place);
                    CrossGeofence.Current.StartMonitoring(new GeofenceCircularRegion(place.Name, place.Latitude, place.Longitude, place.Radius)
                    {
                        //NotifyOnStay = true,
                        //StayedInThresholdDuration = TimeSpan.FromMinutes(5),

                        // test to make it work
                        NotifyOnStay = true,
                        NotifyOnEntry = true,
                        NotifyOnExit = true,
                        ShowNotification = true,
                        ShowEntryNotification = true,
                        ShowExitNotification = true,
                        ShowStayNotification = true,
                        NotificationStayMessage = "stay message!",
                        NotificationEntryMessage = "entering geofence!",
                        NotificationExitMessage = "exiting geofence!",
                        StayedInThresholdDuration = TimeSpan.FromSeconds(60),
                        

                    });

                    MessagingCenter.Send<MapViewModel, Models.Place>(this, "AddPin", place);

                    UserDialogs.Instance.HideLoading();

                    await UserDialogs.Instance.AlertAsync(string.Format("{0} geofence reagion added!", place.Name));

                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    IsEditing = false;
                }
            }
        }


    }
}
