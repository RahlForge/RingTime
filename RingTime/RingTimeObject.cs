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

        public static int[] GetRunDays(List<DayOfWeek> daysOfWeek)
        {
            int[] days = new int[daysOfWeek.Count];
            for(int i = 0; i < daysOfWeek.Count; i++)
            {
                days[i] = (int)daysOfWeek[i];
            }
            return days;
        }

        public static List<DayOfWeek> GetRunDays(int[] daysOfWeek)
        {
            List<DayOfWeek> days = new List<DayOfWeek>();
            for (int i = 0; i < daysOfWeek.Length; i++)
            {
                days.Add((DayOfWeek)daysOfWeek[i]);
            }
            return days;
        }

        public RingTimeObject(DateTime runDateTime, int newRingerVolume, int newNotificationVolume, 
            List<DayOfWeek> daysOfWeek, bool runOnce = false)
        {
            this.runDateTime = runDateTime;
            this.newRingerVolume = newRingerVolume;
            this.newNotificationVolume = newNotificationVolume;
            this.daysOfWeek = daysOfWeek;
        }

        public RingTimeObject(Bundle b)
            :this(DateTime.Parse(b.GetString(bundleRunDateTime)), b.GetInt(bundleRingerVolume), 
                 b.GetInt(bundleNotificationVolume), GetRunDays(b.GetIntArray(bundleDaysOfWeek)))
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

        public void SetNextRunDateTime()
        {
            var today = DateTime.Today;
            if (d == DateTime.Now.DayOfWeek &&
                DateTime.Now.TimeOfDay >= runTime)
                today = today.AddDays(1);
            var daysUntilNextRun = (d - today.DayOfWeek + 7) % 7;
            var nextRun = today.AddDays(daysUntilNextRun);
            return new DateTime(nextRun.Year, nextRun.Month, nextRun.Day, 
                runTime.Hours, runTime.Minutes, runTime.Seconds);
        }

        public void SetNextRunDateTime(DateTime runDateTime)
        {

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