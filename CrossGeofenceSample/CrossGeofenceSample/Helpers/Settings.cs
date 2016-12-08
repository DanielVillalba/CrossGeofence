// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Diagnostics;
using System;

namespace CrossGeofenceSample.Helpers
{
  /// <summary>
  /// This is the Settings static class that can be used in your Core solution or in any
  /// of your client applications. All settings are laid out the same exact way with getters
  /// and setters. 
  /// </summary>
  public static class Settings
  {
    private static ISettings AppSettings
    {
      get
      {
        return CrossSettings.Current;
      }
    }

    #region Setting Constants

    private const string SettingsKey = "settings_key";
    private static readonly string SettingsDefault = string.Empty;

        private const string EventDescriptionKey = "event_description_key";
        private static readonly string EventDescriptionDefault = string.Empty;

        private const string EventIdKey = "event_id_key";
        private static readonly string EventIdDefault = string.Empty;

        private const string EventDateKey = "event_date_key";
        private static readonly string EventDateDefault = string.Empty;

        private const string EventIdsKey = "event_ids";

        private static ISet<string> eventIds = new HashSet<string>();

        private const string IdSeparator = "&";

        #endregion


        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
            }
        }

        public static string EventDescription
        {
            get
            {
                return AppSettings.GetValueOrDefault(EventDescriptionKey, EventDescriptionDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EventDescriptionKey, value);
            }
        }

        public static string EventDate
        {
            get
            {
                return AppSettings.GetValueOrDefault(EventDateKey, EventDateDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(EventDateKey, value);
            }
        }

        public static void SaveResult(Models.Event e)
        {
            AppSettings.AddOrUpdateValue(GetFieldKey(e.Id.ToString(), EventIdKey), e.Id);
            AppSettings.AddOrUpdateValue(GetFieldKey(e.Id.ToString(), EventDescriptionKey), e.Description);
            AppSettings.AddOrUpdateValue(GetFieldKey(e.Id.ToString(), EventDateKey), e.Date);


            eventIds.Add(e.Id.ToString());

            AppSettings.AddOrUpdateValue(EventIdsKey, string.Join(IdSeparator, eventIds.ToArray()));

        }
        public static void RemoveEvent(Models.Event e)
        {
            AppSettings.Remove(GetFieldKey(e.Id.ToString(), EventIdKey));
            AppSettings.Remove(GetFieldKey(e.Id.ToString(), EventDescriptionKey));
            AppSettings.Remove(GetFieldKey(e.Id.ToString(), EventDateKey));

            eventIds.Remove(e.Id.ToString());

            AppSettings.AddOrUpdateValue(EventIdsKey, string.Join(IdSeparator, eventIds.ToArray()));
        }
        public static void ClearAllEvents()
        {
            foreach (string key in eventIds)
            {
                AppSettings.Remove(GetFieldKey(key, EventDescriptionKey));
            }
            eventIds.Clear();

            AppSettings.Remove(EventIdsKey);
        }
        public static ObservableCollection<Models.Event> GetEvents()
        {

            ObservableCollection<Models.Event> events = new ObservableCollection<Models.Event>();

            if (!string.IsNullOrEmpty(AppSettings.GetValueOrDefault(EventIdsKey, string.Empty)))
            {

                string[] keys = AppSettings.GetValueOrDefault(EventIdsKey, string.Empty).ToString().Split(IdSeparator[0]);

                foreach (string k in keys)
                {
                    Debug.WriteLine(k);
                    eventIds.Add(k);
                    int id = AppSettings.GetValueOrDefault(GetFieldKey(k, EventIdKey), 0);
                    string date = AppSettings.GetValueOrDefault(GetFieldKey(k, EventDateKey), string.Empty);
                    string description = AppSettings.GetValueOrDefault(GetFieldKey(k, EventDescriptionKey), string.Empty);
                    events.Add(new Models.Event()
                    {
                        Id = id,
                        Description = description,
                        Date = date
                    });
                }

            }
            return new ObservableCollection<Models.Event>(events.OrderBy(p => p.Id));

        }
        static string GetFieldKey(string id, string fieldName)
        {
            return "GeoSample" + "_" + id + "_" + fieldName;
        }

    }
}