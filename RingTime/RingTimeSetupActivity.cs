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
    [Activity(Label = "Set up RingTime")]
    public class RingTimeSetupActivity : Activity                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
    {
        const int TIME_DIALOG_ID = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.RingTimeSetup);

            var selectDateButton = FindViewById<Button>(Resource.Id.selectDateButton);
            selectDateButton.Click += SelectDateButton_Click;

            var setTimeButton = FindViewById<Button>(Resource.Id.ringTimeButton);
            setTimeButton.Click += (o, e) => ShowDialog(TIME_DIALOG_ID);
        }

        private void SelectDateButton_Click(object sender, EventArgs e)
        {
            var dateText = FindViewById<TextView>(Resource.Id.specificDateText);
            DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
            {
                dateText.Text = time.ToShortDateString();
            });

            frag.Show(FragmentManager, DatePickerFragment.TAG);
        }

        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            string timeString = string.Format("{0}:{1}", e.HourOfDay, e.Minute.ToString().PadLeft(2, '0'));
            DateTime time = DateTime.Parse(timeString);            
            var timeText = FindViewById<TextView>(Resource.Id.ringTimeText);
            timeText.Text = time.ToShortTimeString();
        }

        protected override Dialog OnCreateDialog(int id)
        {
            if (id == TIME_DIALOG_ID)
                return new TimePickerDialog(this, TimePickerCallback, DateTime.Now.Hour, DateTime.Now.Minute, false);

            return null;
        }
    }
}