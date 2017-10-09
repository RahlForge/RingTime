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
    enum RingTimeFrequency { EveryDay, CertainDays, SpecificDate };

    public class RingTimeObject
    {
        // Every Day, Certain Days, Specific Date
        private string ringName; // Name of ringer
        private List<DayOfWeek> certainDays; // A set of days to run the service
        private DateTime specificDate; // A specific date to run the service
        private RingTimeFrequency ringTimeFrequency; // The frequency with which the service will run
        private DateTime ringTime; // The time to set the new volume levels
        private int newRingerVolume; // The new ringer volume to set
        private int newNotificationVolume; // The new notification volume to set

        public RingTimeObject(DateTime ringTime, int newRingerVolume, int newNotificationVolume)
        {
            this.ringTime = ringTime;
            this.newRingerVolume = newRingerVolume;
            this.newNotificationVolume = newNotificationVolume;
            ringTimeFrequency = RingTimeFrequency.EveryDay;
        }

        public RingTimeObject(DateTime ringTime, int newRingerVolume, int newNotificationVolume, List<DayOfWeek> certainDays) 
            :this(ringTime, newRingerVolume, newNotificationVolume)
        {
            this.certainDays = certainDays;
            ringTimeFrequency = RingTimeFrequency.CertainDays;

        }

        public RingTimeObject(DateTime ringTime, int newRingerVolume, int newNotificationVolume, DateTime specificDate)
            :this(ringTime, newRingerVolume, newNotificationVolume)
        {
            this.ringTime = ringTime;
            this.newRingerVolume = newRingerVolume;
            this.newNotificationVolume = newNotificationVolume;
            this.specificDate = specificDate;
            ringTimeFrequency = RingTimeFrequency.SpecificDate;
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
        public DateTime GetRunTime()
        {
            return ringTime;
        }


        public DateTime GetRunDate()
        {
            return specificDate;
        }


        public List<DayOfWeek> GetRunDays()
        {
            return certainDays;
        }

        public string GetName()
        {
            return ringName;
        }

        public Bundle ToBundle()
        {
            Bundle b = new Bundle();
            b.PutString("ringname", GetName());
            b.PutString("rundate", GetRunDate().ToShortDateString());
            b.PutString("runtime", GetRunTime().ToShortTimeString());
            b.PutString("ringervolume", GetNewRingerVolume().ToString());
            b.PutString("notificationvolume", GetNewNotificationVolume().ToString());
            return b;
        }
    }
}