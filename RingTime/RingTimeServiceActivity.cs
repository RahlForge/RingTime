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

namespace RingTime
{
    [Activity(Label = "RingTimeServiceActivity")]
    public class RingTimeServiceActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            // Prepare the AlarmManager and new Intent for creating the alarm service
            AlarmManager manager = (AlarmManager)GetSystemService(AlarmService);
            Intent ringTimeIntent = new Intent(this, typeof(RingTimeReceiver));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(this, 0, ringTimeIntent, 
                PendingIntentFlags.UpdateCurrent);
            var rto = new RingTimeObject(savedInstanceState.GetBundle("ringtime"));



            manager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + 5 * 1000, 
                pendingIntent);
        }
    }
}