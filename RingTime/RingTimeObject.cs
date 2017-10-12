using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;
using Android.Content.Res;

namespace RingTime
{
    public enum RingTimeFrequency { EveryDay, CertainDays, SpecificDate };

    public class RingTimeObject
    {
        // Every Day, Certain Days, Specific Date
        protected string ringName; // Name of ringer
        protected List<DayOfWeek> daysOfWeek; // A set of days to run the service
        protected DateTime runDateTime; // The date and time to run the service     
        protected bool runOnce; // Should this service be run only one time?
        protected int newRingerVolume; // The new ringer volume to set
        protected int newNotificationVolume; // The new notification volume to set     

        // Define parameters for bundle strings
        public static string bundleRingTimeObject = "ringtimeobject";
        public static string bundleRingName = "ringname";
        public static string bundleRunDateTime = "rundatetime";
        public static string bundleRingerVolume = "ringervolume";
        public static string bundleNotificationVolume = "notificationvolume";
        public static string bundleDaysOfWeek = "daysofweek";

        public RingTimeObject(DateTime runDateTime, int newRingerVolume, int newNotificationVolume, bool runOnce = false)
        {
            this.runDateTime = runDateTime;
            this.newRingerVolume = newRingerVolume;
            this.newNotificationVolume = newNotificationVolume;
            this.daysOfWeek = daysOfWeek;
        }

        public RingTimeObject(Bundle b)
            :this(DateTime.Parse(b.GetString(bundleRunDateTime)), b.GetInt(bundleRingerVolume), b.GetInt(bundleNotificationVolume))
        {            
        }

        /// <summary>
        /// Returns the integer value of the new ringer volume (0-100)
        /// </summary>
        /// <returns>newRingerVolume</returns>
        public int GetNewRingerVolume()
        {
            return newRingerVolume;
        }

        /// <summary>
        /// Returns the integer value of the new notification volume (0-100)
        /// </summary>
        /// <returns>newNotificationVolume</returns>
        public int GetNewNotificationVolume()
        {
            return newNotificationVolume;
        }

        /// <summary>
        /// Retrieves the DateTime object holding the repeatable alarm time
        /// </summary>
        /// <returns>newRingerVolume</returns>
        public string GetRunTime()
        {
            return runDateTime.ToShortTimeString();
        }


        public string GetRunDate()
        {
            return runDateTime.ToShortDateString();
        }

        public string GetRunDateTime()
        {
            return runDateTime.ToString();
        }


        public List<DayOfWeek> GetRunDays()
        {
            return daysOfWeek;
        }

        public string GetName()
        {
            return ringName;
        }

        public Bundle ToBundle()
        {
            Bundle b = new Bundle();
            b.PutString(bundleRingName, GetName());
            b.PutString(bundleRunDateTime, GetRunDateTime());
            b.PutInt(bundleRingerVolume, GetNewRingerVolume());
            b.PutInt(bundleNotificationVolume, GetNewNotificationVolume());
            return b;
        }
    }
}