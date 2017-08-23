using Android.App;
using Android.Widget;
using Android.OS;
using static Android.Resource;
using Android.Views;
using Android.Content;

namespace RingTime
{
    [Activity(Label = "RingTime", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ListActivity
    {
        string[] items;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Build list view
            items = Resources.GetStringArray(Resource.Array.ringTimes);
            ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items);
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            var t = items[position];
            if (t == Resources.GetString(Resource.String.addRingTime))
            {
                //Toast.MakeText(this, t, ToastLength.Short).Show();
                Intent intent = new Intent(this, typeof(RingTimeSetupActivity));
                StartActivity(intent);
            }
            //base.OnListItemClick(l, v, position, id);
        }
    }
}

