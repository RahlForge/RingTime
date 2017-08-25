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

namespace RingTime
{
    enum RingTimeFrequency { EveryDay, CertainDays, SpecificDate };

    public class RingTimeObject
    {
        // Every Day, Certain Days, Specific Date
        private List<DayOfWeek> certainDays;
        private DateTime specificDate;
        private RingTimeFrequency ringTimeFrequency;
        private DateTime ringTime;        
        private int newRingerVolume;
        private int newNotificationVolume;

        public RingTimeObject(DateTime ringTime, int newRingerVolume, int newNotificationVolume, bool everyDay = true)
        {
            this.ringTime = ringTime;
            this.newRingerVolume = newRingerVolume;
            this.newNotificationVolume = newNotificationVolume;
        }

        public RingTimeObject(DateTime ringTime, int newRingerVolume, int newNotificationVolume, List<DayOfWeek> certainDays) 
            :this(ringTime, newRingerVolume, newNotificationVolume, false)
        {
            this.certainDays = certainDays;
        }

        public RingTimeObject(DateTime ringTime, int newRingerVolume, int newNotificationVolume, DateTime specificDate)
            :this(ringTime, newRingerVolume, newNotificationVolume, false)
        {
            this.specificDate = specificDate;
        }
    }
}