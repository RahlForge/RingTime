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
using Android.Media;
using Java.Util;

namespace RingTime
{
    [BroadcastReceiver]
    public class RingTimeReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Toast.MakeText(context, "RingTime activated", ToastLength.Short).Show();
            /*
            AudioManager mgr = (AudioManager)context.GetSystemService(Context.AudioService);
            mgr.SetStreamVolume(Stream.Ring, intent.GetIntExtra("ringer_volume", 0), 0);
            mgr.SetStreamVolume(Stream.Notification, intent.GetIntExtra("notification_volume", 0), 0);
            */
        }
    }
}